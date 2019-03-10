using HarfBuzzSharp;
using SkiaSharp;
using SkiaSharp.HarfBuzz;
using System;
using System.Linq;
using System.Text;

namespace SkiaMonospace
{
    public struct Screenchar
    {
        public char Character { get; set; }
        public SKColor Forecolor { get; set; } 
        public SKColor Backcolor { get; set; }
        public uint Attributes { get; set; }
    }

    public class SkiaMonospaceRenderer : IDisposable
    {
        internal const int FONT_SIZE_SCALE = 512;
        private HarfBuzzSharp.Buffer buffer;
        private Font _font;
        private float _lineHeight;

        SKPaint _currentPaint;
        int _widthInCharacters;
        int _heightInCharacters;
        SKSize _preferredSize;
        Screenchar[] _screenBuffer;
        readonly char _clearScreenCharacter = '1';

        public SkiaMonospaceRenderer(SKTypeface typeface, float textSize,
                                     SKColor currentForecolor, SKColor currentBackcolor,
                                     int widthInCharacters, int heightInCharacters)
        {
            _widthInCharacters = widthInCharacters;
            _heightInCharacters = heightInCharacters;
            CurrentForecolor = currentForecolor;
            CurrentBackcolor = CurrentBackcolor;
            _screenBuffer = new Screenchar[_widthInCharacters * _heightInCharacters];

            _currentPaint = new SKPaint()
            {
                Style = SKPaintStyle.Fill,
                Color = CurrentForecolor,
                IsAntialias = true,
                Typeface = typeface,
                TextSize = textSize
            };

            var fMetrics = _currentPaint.FontMetrics;
            _lineHeight = fMetrics.CapHeight + fMetrics.XHeight +
                          fMetrics.Bottom + fMetrics.Descent;
            _preferredSize = new SKSize(fMetrics.MaxCharacterWidth * _widthInCharacters,
                                        _lineHeight * _heightInCharacters);

            int index;
            using (var blob = typeface.OpenStream(out index).ToHarfBuzzBlob())
            using (var face = new Face(blob, (uint)index))
            {
                face.Index = (uint)index;
                face.UnitsPerEm = (uint)typeface.UnitsPerEm;

                _font = new Font(face);
                _font.SetScale(FONT_SIZE_SCALE, FONT_SIZE_SCALE);
                _font.SetFunctionsOpenType();
            }

            ClearScreen();
        }

        public void ClearScreen()
        {
            for (var i = 0; i < _screenBuffer.Length - 1; i++)
            {
                _screenBuffer[i].Backcolor = CurrentBackcolor;
                _screenBuffer[i].Forecolor = CurrentForecolor;
                _screenBuffer[i].Character = _clearScreenCharacter;
            }
        }

        public void Render(SKSurface surface)
        {
            // TODO:001 Refactor SKShaperClone to render screenbuffer content to Screen. 
            // For this to end, we have SKShaperClone as a template for the Points calculation.
            var shaper = new SKShaper(_currentPaint.Typeface);
            float xOffset = 0, yOffset = 0;
            var totalCount = 0;

            SKPoint[] points = null;
            uint[] clusters = null;
            uint[] codepoints = null;

            buffer = new HarfBuzzSharp.Buffer();

            for (int lineCount = 0; lineCount < _heightInCharacters; lineCount++)
            {
                yOffset += _lineHeight;

                var sb = new StringBuilder();

                for (int columnCount = 0; columnCount < _widthInCharacters; columnCount++)
                {
                    sb.Append(ScreenBuffer[totalCount++].Character);
                }

                var text = sb.ToString();

                // add the text to the buffer
                buffer.ClearContents();
                buffer.AddUtf8(text);

                // try to understand the text
                buffer.GuessSegmentProperties();

                // do the shaping
                _font.Shape(buffer);

                // get the shaping results
                var len = buffer.Length;
                var info = buffer.GlyphInfos;
                var pos = buffer.GlyphPositions;

                // get the sizes
                float textSizeY = _currentPaint.TextSize / FONT_SIZE_SCALE;
                float textSizeX = textSizeY * _currentPaint.TextScaleX;

                points = new SKPoint[len];
                clusters = new uint[len];
                codepoints = new uint[len];

                for (var i = 0; i < len; i++)
                {
                    codepoints[i] = info[i].Codepoint;

                    clusters[i] = info[i].Cluster;

                    points[i] = new SKPoint(
                        xOffset + pos[i].XOffset * textSizeX,
                        yOffset - pos[i].YOffset * textSizeY);

                    // move the cursor
                    xOffset += pos[i].XAdvance * textSizeX;
                    yOffset += pos[i].YAdvance * textSizeY;
                }
                var bytes = codepoints.Select(cp => BitConverter.GetBytes((ushort)cp)).SelectMany(b => b).ToArray();
                surface.Canvas.DrawPositionedText(bytes, points, _currentPaint);
                xOffset = 0;
            }
        }

        public Screenchar[] ScreenBuffer { get => _screenBuffer; }

        public SKColor CurrentForecolor { get; set; }
        public SKColor CurrentBackcolor { get; set; }
        public SKSize PreferredSize => _preferredSize;

        private bool isDisposed = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    _font?.Dispose();
                    buffer?.Dispose();
                }

                isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}

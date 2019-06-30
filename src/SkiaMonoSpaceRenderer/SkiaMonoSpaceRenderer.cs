using HarfBuzzSharp;
using OpenTK.Graphics.OpenGL;
using SkiaSharp;
using SkiaSharp.HarfBuzz;
using System;
using System.Linq;
using System.Runtime.InteropServices;
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
        internal const int FONT_SIZE_SCALE = 256;
        private Font _font;
        private float _lineHeight;

        private SKPaint _currentPaint;
        private int _widthInCharacters;
        private int _heightInCharacters;
        private float _deviceDpi;
        private SKSize _preferredSize;
        private Screenchar[] _screenBuffer;
        private readonly char _clearScreenCharacter = ' ';
        private (float width, float xAdvance) _measureTextWidthResult;

        Blob _blob;
        Face _face;

        public SkiaMonospaceRenderer(SKTypeface typeface, float textSize,
                                     SKColor currentForecolor, SKColor currentBackcolor,
                                     int widthInCharacters, int heightInCharacters, float deviceDpi)
        {
            _widthInCharacters = widthInCharacters;
            _heightInCharacters = heightInCharacters;
            CurrentForecolor = currentForecolor;
            CurrentBackcolor = CurrentBackcolor;
            _screenBuffer = new Screenchar[_widthInCharacters * _heightInCharacters];
            _deviceDpi = deviceDpi;

            _currentPaint = new SKPaint()
            {
                Style = SKPaintStyle.Fill,
                Color = CurrentForecolor,
                IsAntialias = true,
                Typeface = typeface,
                TextSize = textSize,
                TextEncoding = SKTextEncoding.Utf32
            };

            var fMetrics = _currentPaint.FontMetrics;
            _lineHeight = textSize;

            int index;
            _blob = typeface.OpenStream(out index).ToHarfBuzzBlob();
            _face = new Face(_blob, (uint)index)
            {
                Index = (uint)index,
                UnitsPerEm = (uint)typeface.UnitsPerEm
            };

            _font = new Font(_face);
            _font.SetScale(FONT_SIZE_SCALE, FONT_SIZE_SCALE);
            _font.SetFunctionsOpenType();
            _measureTextWidthResult = MeasureTextWidth("W");
            _preferredSize = new SKSize(_measureTextWidthResult.width  * _widthInCharacters,
                            _lineHeight * _heightInCharacters);

            ClearScreen(_clearScreenCharacter);
        }

        public void ClearScreen(char clearCharacter)
        {
            for (var i = 0; i < _screenBuffer.Length ; i++)
            {
                _screenBuffer[i].Backcolor = CurrentBackcolor;
                _screenBuffer[i].Forecolor = CurrentForecolor;
                _screenBuffer[i].Character = clearCharacter;
            }
        }

        private void BuildRenderBuffers()
        {
        }

        private void BuildMainGlyphPositionsBuffer(SKPaint paint)
        {
            float textSizeY = paint.TextSize / FONT_SIZE_SCALE;
            float textSizeX = textSizeY * paint.TextScaleX;

            float xOffset = 0, yOffset = 0;

            _mainGlyphPositionsBuffer = new GlyphsRenderInfo(_screenBuffer.Length);

            int count = 0;

            for (int lineCount = 0; lineCount < _heightInCharacters; lineCount++)
            {
                for (int columnCount = 0; columnCount < _widthInCharacters; columnCount++)
                {
                    _mainGlyphPositionsBuffer.GlyphPositions[count] = new SKPoint(
                        xOffset * textSizeX,
                        yOffset * textSizeY);

                    // move the cursor
                    xOffset += _measureTextWidthResult.xAdvance * textSizeX;
                }
                yOffset += _lineHeight;
            }
        }

        public void Render(SKSurface surface)
        {
            var shaper = new SKShaper(_currentPaint.Typeface);
            float xOffset = 0, yOffset = 0;
            var totalCount = 0;

            SKPoint[] points = null;
            Byte[] textBytes;

            surface.Canvas.Clear();

            for (int lineCount = 0; lineCount < _heightInCharacters; lineCount++)
            {
                using (var buffer = new HarfBuzzSharp.Buffer())
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

                    for (var i = 0; i < len; i++)
                    {
                        points[i] = new SKPoint(
                            xOffset + pos[i].XOffset * textSizeX,
                            yOffset - pos[i].YOffset * textSizeY);

                        // move the cursor
                        xOffset += pos[i].XAdvance * textSizeX;
                        yOffset += pos[i].YAdvance * textSizeY;
                    }

                    textBytes = Encoding.UTF32.GetBytes(text);
                    surface.Canvas.DrawPositionedText(textBytes, points, _currentPaint);

                    if (_currentPaint.Color == SKColors.White)
                        _currentPaint.Color = SKColors.LightBlue;
                    else
                        _currentPaint.Color = SKColors.White;
                }

                xOffset = 0;
            }
        }

        private (float width, float xAdvance) MeasureTextWidth(string text)
        {
            float xOffset = 0;

            using (var buffer = new HarfBuzzSharp.Buffer())
            {
                // add the text to the buffer
                buffer.ClearContents();
                buffer.AddUtf8(text);

                // try to understand the text
                buffer.GuessSegmentProperties();

                // do the shaping
                _font.Shape(buffer);

                // get the shaping results
                var len = buffer.Length;
                var pos = buffer.GlyphPositions;

                // get the sizes
                float textSizeY = _currentPaint.TextSize / FONT_SIZE_SCALE;
                float textSizeX = textSizeY * _currentPaint.TextScaleX;

                for (var i = 0; i < len; i++)
                {
                    // move the cursor
                    xOffset += pos[i].XAdvance * textSizeX;
                }

                return (xOffset, pos[0].XAdvance);
            }
        }

        public Screenchar[] ScreenBuffer { get => _screenBuffer; }

        public SKColor CurrentForecolor { get; set; }
        public SKColor CurrentBackcolor { get; set; }
        public SKSize PreferredSize => _preferredSize;

        private bool isDisposed = false; // To detect redundant calls
        private GlyphsRenderInfo _mainGlyphPositionsBuffer;

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    _font?.Dispose();
                    _face.Dispose();
                    _blob.Dispose();
                }

                isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }

    internal class GlyphsRenderInfo
    {
        public byte[] TextBytes { get; private set; }
        public SKPoint[] GlyphPositions { get; private set; }

        public GlyphsRenderInfo(int charCount)
        {
            TextBytes = new byte[charCount];
            GlyphPositions = new SKPoint[charCount];
        }
    }
}

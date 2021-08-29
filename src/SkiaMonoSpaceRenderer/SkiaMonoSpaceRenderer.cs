using SkiaSharp;
using System;
using System.Diagnostics;
using System.Text;

namespace SkiaMonospace
{
    public struct Screenchar
    {
        public char[] Characters { get; set; }
        public SKColor Forecolor { get; set; }
        public SKColor Backcolor { get; set; }
        public uint Attributes { get; set; }
    }

    public class SkiaMonospaceRenderer 
    {
        private readonly float _lineHeight;

        private readonly SKPaint _currentPaint;
        private readonly int _widthInCharacters;
        private readonly int _heightInCharacters;
        private readonly Screenchar[] _screenBuffer;
        private readonly SKTextBlob[] _screenBlobs;
        private readonly (SKRunBuffer runBuffer, float x, float y)[] _screenRuns;
        private readonly char _clearScreenCharacter = ' ';

        private readonly SKFont _skFont;

        private (float width, float xAdvance) _measureTextWidthResult;
        private SKSize _preferredSize;
        private Stopwatch _stopWatch;

        public SkiaMonospaceRenderer(SKTypeface typeface, float textSize,
                                     SKColor currentForecolor, SKColor currentBackcolor,
                                     int widthInCharacters, int heightInCharacters)
        {
            CurrentForecolor = currentForecolor;
            CurrentBackcolor = currentBackcolor;

            var unitsPerEm = typeface.UnitsPerEm;

            _widthInCharacters = widthInCharacters;
            _heightInCharacters = heightInCharacters;
            _screenBuffer = new Screenchar[_widthInCharacters * _heightInCharacters];
            _screenBlobs = new SKTextBlob[_widthInCharacters * _heightInCharacters];
            _screenRuns = new (SKRunBuffer, float, float)
                [_widthInCharacters * _heightInCharacters];

            _currentPaint = new SKPaint()
            {
                Style = SKPaintStyle.Fill,
                Color = CurrentForecolor,
                IsAntialias = true,
                Typeface = typeface,
                TextSize = textSize,
                TextEncoding = SKTextEncoding.Utf32
            };

            _lineHeight = textSize;

            _skFont = new SKFont(typeface, textSize);
            var charWidth = _skFont.MeasureText(typeface.GetGlyphs("XXXXXXXXXX"), _currentPaint) / 10;

            _measureTextWidthResult = (charWidth, charWidth);
            _preferredSize = new SKSize(_measureTextWidthResult.width * _widthInCharacters,
                            _lineHeight * _heightInCharacters);

            BuildGlyphBuffers();
            ClearScreen(_clearScreenCharacter);
        }

        public void ClearScreen(char clearCharacter)
        {
            for (var i = 0; i < _screenBuffer.Length; i++)
            {
                _screenBuffer[i].Backcolor = CurrentBackcolor;
                _screenBuffer[i].Forecolor = CurrentForecolor;
                _screenBuffer[i].Characters = new char[] { clearCharacter };
            }
        }

        private void BuildGlyphBuffers()
        {
            float textSizeY = _currentPaint.TextSize;
            float textSizeX = _currentPaint.TextScaleX;

            var typeFace = _currentPaint.Typeface;
            SKColor currentForeColor = _screenBuffer[0].Forecolor;
            SKColor currentBackColor = _screenBuffer[0].Backcolor;
            int screenBufferIndex;
            float currentGlyphPosition, currentLinePosition;

            var blob = new SKTextBlobBuilder();
            screenBufferIndex = 0;
            currentLinePosition = 0;
            Screenchar screenChar;

            for (int y = 0; y < _heightInCharacters; y++)
            {
                currentGlyphPosition = 0;

                for (int x = 0; x < _widthInCharacters; x++)
                {
                    screenChar = _screenBuffer[screenBufferIndex];
                    _screenRuns[screenBufferIndex].x = currentGlyphPosition;
                    _screenRuns[screenBufferIndex++].y = currentLinePosition;
                    currentGlyphPosition += _measureTextWidthResult.xAdvance * textSizeX;
                }
                currentLinePosition += _lineHeight;
                currentGlyphPosition = 0;
            }
        }

        private void UpdateAllGlyphBuffers()
        {
            var typeFace = _currentPaint.Typeface;

            for (int screenBufferIndex = 0; screenBufferIndex < ScreenBuffer.Length; screenBufferIndex++)
            {
                var screenBuffer = _screenBuffer[screenBufferIndex];
                var tempBlob = SKTextBlob.Create(
                    screenBuffer.Characters.AsSpan(), 
                    _skFont, 
                    new SKPoint(
                        _screenRuns[screenBufferIndex].x, 
                        _screenRuns[screenBufferIndex].y));
                _screenBlobs[screenBufferIndex] = tempBlob;
            }
        }

        private void RenderGlyphBuffers(SKCanvas canvas)
        {
            _stopWatch = Stopwatch.StartNew();
            UpdateAllGlyphBuffers();

            for (int screenBufferIndex = 0; screenBufferIndex < ScreenBuffer.Length; screenBufferIndex++)
            {
                canvas.DrawText(
                    _screenBlobs[screenBufferIndex], 0, 0, _currentPaint);
            }
            _stopWatch.Stop();
            LastFrameInMs = _stopWatch.ElapsedMilliseconds;
        }

        public void Render(SKSurface surface)
        {
            surface.Canvas.Clear();
            RenderGlyphBuffers(surface.Canvas);
        }

        public Screenchar[] ScreenBuffer { get => _screenBuffer; }

        public SKColor CurrentForecolor { get; set; }
        public SKColor CurrentBackcolor { get; set; }
        public SKSize PreferredSize => _preferredSize;

        public long LastFrameInMs { get; private set; }
    }

    internal class GlyphsRenderInfo
    {
        public byte[] TextBytes { get; private set; }
        public SKPoint[] GlyphPositions { get; private set; }


        public GlyphsRenderInfo(int charCount)
        {
            // Works, because we use UTF-32.
            TextBytes = new byte[charCount * 4];
            GlyphPositions = new SKPoint[charCount];
        }
    }
}

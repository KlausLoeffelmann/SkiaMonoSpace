using HarfBuzzSharp;
using SkiaSharp;
using SkiaSharp.HarfBuzz;
using System;

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
        private HarfBuzzSharp.Buffer _buffer;
        private Font _font;

        SKPaint _currentPaint;
        int _widthInCharacters;
        int _heightInCharacters;
        SKSize _preferredSize;
        Screenchar[] _screenBuffer;
        readonly char _clearScreenCharacter = 'A';

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
            _preferredSize = new SKSize(fMetrics.MaxCharacterWidth * _widthInCharacters,
                                        (fMetrics.CapHeight +
                                         fMetrics.XHeight + fMetrics.Bottom + fMetrics.Descent)
                                        * _heightInCharacters);

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

            _buffer = new HarfBuzzSharp.Buffer();

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
            //var result = shaper.Shape(text, x, y, _currentPaint);
            //surface.Canvas.DrawShapeResultText(result, X, Y, _currentPaint);
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
                    _buffer?.Dispose();
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

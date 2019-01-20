using SkiaSharp;
using SkiaSharp.HarfBuzz;

namespace SkiaMonospace
{
    public struct Screenchar
    {
        public char Character { get; set; }
        public SKColor Forecolor { get; set; } 
        public SKColor Backcolor { get; set; }
        public uint Attributes { get; set; }
    }

    public class SkiaMonospaceRenderer
    {
        SKPaint _currentPaint;
        int _widthInCharacters;
        int _heightInCharacters;
        float _preferredWidth;
        float _preferredHeight;
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
            var charWidth = fMetrics.MaxCharacterWidth;
            var charHeight = fMetrics.XHeight;
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
            // TODO:001 Refactor SKShaperClone to render screenbuffer content to Screen. For this to end, we have SKShaperClone as a template for the Points calculation.
            var shaper = new SKShaper(_currentPaint.Typeface);
            //var result = shaper.Shape(text, x, y, _currentPaint);
            //surface.Canvas.DrawShapeResultText(result, X, Y, _currentPaint);
        }

        public Screenchar[] ScreenBuffer { get => _screenBuffer; }

        public SKColor CurrentForecolor { get; set; }
        public SKColor CurrentBackcolor { get; set; }
    }
}

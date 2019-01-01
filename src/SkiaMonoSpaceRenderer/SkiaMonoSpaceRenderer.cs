using HarfBuzzSharp;
using SkiaSharp;
using SkiaSharp.HarfBuzz;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace SkiaMonoSpaceRenderer
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
        readonly char _clearScreenCharacter = ' ';

        public SkiaMonospaceRenderer(SKTypeface typeface, float textSize,
                                     SKColor foregroundColor, SKColor backgroundColor,
                                     int widthInCharacters, int hightInCharacters)
        {
            _currentPaint = new SKPaint()
            {
                Style = SKPaintStyle.Fill,
                Color = foregroundColor,
                IsAntialias = true,
                Typeface = typeface,
                TextSize = textSize
            };

            var fMetrics = _currentPaint.FontMetrics;
            var charWidth = fMetrics.MaxCharacterWidth;
            var charHeight = fMetrics.XHeight;
            _screenBuffer = new Screenchar[widthInCharacters * _heightInCharacters];
        }

        public void ClearScreen(SKColor forecolor, SKColor backcolor)
        {
            for (var i = 0; i < _screenBuffer.Length - 1; i++)
            {
                _screenBuffer[i].Backcolor = backcolor;
                _screenBuffer[i].Forecolor = forecolor;
                _screenBuffer[i].Character = _clearScreenCharacter;
            }
        }

        public void Render(SKSurface surface)
        {
            var shaper = new SKShaper(_currentPaint.Typeface);
            //var result = shaper.Shape(text, x, y, _currentPaint);
            //surface.Canvas.DrawShapeResultText(result, X, Y, _currentPaint);
        }

        public Screenchar[] ScreenBuffer { get => _screenBuffer; }
    }
}

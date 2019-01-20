using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace SkiaMonospaceRenderer
{
    public static class ColorExtensions
    {
        public static Color ToFormsColor(this SKColor color)
        {
            return Color.FromArgb(color.Alpha,
                                  color.Red,
                                  color.Green,
                                  color.Blue);
        }

        public static SKColor ToSKColor(this Color color)
        {
            return new SKColor(color.R,
                               color.G,
                               color.B, 
                               color.A);
        }
    }
}

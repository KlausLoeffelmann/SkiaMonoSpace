using SkiaSharp;
using System;
using SkiaSharp.HarfBuzz;
using System.Linq;

namespace SkiaEditComponent
{
    public static class CanvasExtensions
    {
        public static SKShaper.Result DrawShapedText(this SKCanvas canvas, string text, 
            SKShaper shaper, float x, float y, SKPaint paint)
        {
            if (canvas == null)
                throw new ArgumentNullException(nameof(canvas));
            if (shaper == null)
                throw new ArgumentNullException(nameof(shaper));
            if (text == null)
                throw new ArgumentNullException(nameof(text));
            if (paint == null)
                throw new ArgumentNullException(nameof(paint));

            if (string.IsNullOrEmpty(text))
                return null;

            // shape the text
            var resultToReturn = shaper.Shape(text, x, y, paint);

            // draw the text

            using (var paintClone = paint.Clone())
            {
                paintClone.TextEncoding = SKTextEncoding.GlyphId;
                paintClone.Typeface = shaper.Typeface;

                var bytes = resultToReturn.Codepoints.Select(cp => BitConverter.GetBytes((ushort)cp)).SelectMany(b => b).ToArray();
                canvas.DrawPositionedText(bytes, resultToReturn.Points, paintClone);
            }

            return resultToReturn;
        }

        public static void DrawShapeResultText(this SKCanvas canvas, SKShaper.Result result, 
            float x, float y, SKPaint paint)
        {
            if (canvas == null)
                throw new ArgumentNullException(nameof(canvas));
            if (result == null)
                throw new ArgumentNullException(nameof(result));
            if (paint == null)
                throw new ArgumentNullException(nameof(paint));

            // draw the text
            var tf = paint.Typeface;
            paint.TextEncoding = SKTextEncoding.GlyphId;
            paint.Typeface = tf;

            var bytes = result.Codepoints.Select(cp => BitConverter.GetBytes((ushort)cp)).SelectMany(b => b).ToArray();
            canvas.DrawPositionedText(bytes, result.Points, paint);
        }
    }
}


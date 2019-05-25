using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System.Drawing;

namespace SkiaMonospace.Control
{
    internal class SkiaMonospaceRenderTarget : SKGLControl
    {
        internal SkiaMonospaceRenderer _monoSpaceRenderer;

        internal SkiaMonospaceRenderTarget(SKTypeface typeface, float textSize,
                                     SKColor foregroundColor, SKColor backgroundColor,
                                     int widthInCharacters, int heightInCharacters)
        {

            _monoSpaceRenderer = new SkiaMonospaceRenderer(typeface, textSize,
                                                           foregroundColor, backgroundColor,
                                                           widthInCharacters, heightInCharacters);
            this.ClientSize = new Size((int) _monoSpaceRenderer.PreferredSize.Width,
                                       (int) _monoSpaceRenderer.PreferredSize.Height);
        }

        protected override void OnPaintSurface(SKPaintGLSurfaceEventArgs e)
        {
            //base.OnPaintSurface(e);
            _monoSpaceRenderer.Render(e.Surface);
        }
    }
}


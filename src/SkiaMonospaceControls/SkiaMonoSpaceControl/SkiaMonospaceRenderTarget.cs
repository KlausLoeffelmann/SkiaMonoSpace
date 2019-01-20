using SkiaSharp;
using SkiaSharp.Views.Desktop;


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
        }

        protected override void OnPaintSurface(SKPaintGLSurfaceEventArgs e)
        {
            base.OnPaintSurface(e);
            _monoSpaceRenderer.Render(e.Surface);
        }
    }
}


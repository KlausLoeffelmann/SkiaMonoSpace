using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SkiaMonospace.Control
{
    internal class SkiaMonospaceRenderTarget : SKGLControl
    {
        internal SkiaMonospaceRenderer _monoSpaceRenderer;

        internal SkiaMonospaceRenderTarget(SKTypeface typeface, float textSize,
                                     SKColor foregroundColor, SKColor backgroundColor,
                                     int widthInCharacters, int heightInCharacters)
        {
            this.AutoSize = true;
            _monoSpaceRenderer = new SkiaMonospaceRenderer(typeface, textSize,
                                                           foregroundColor, backgroundColor,
                                                           widthInCharacters, heightInCharacters,
                                                           this.DeviceDpi);
        }

        protected override void OnPaintSurface(SKPaintGLSurfaceEventArgs e)
        {
            //base.OnPaintSurface(e);
            _monoSpaceRenderer.Render(e.Surface);
        }

        /// <summary>
        /// Method that forces the control to resize itself when in AutoSize following
        /// a change in its state that affect the size.
        /// </summary>
        private void ResizeForAutoSize()
        {
            if (this.AutoSize)
                this.SetBoundsCore(this.Left, this.Top, this.Width, this.Height,
                            BoundsSpecified.Size);
        }

        /// <summary>
        /// Calculate the required size of the control if in AutoSize.
        /// </summary>
        /// <returns>Size.</returns>
        private Size GetAutoSize()
        {
            //  Do your specific calculation here...
            return new Size((int)_monoSpaceRenderer.PreferredSize.Width,
                                       (int)_monoSpaceRenderer.PreferredSize.Height);
        }

        /// <summary>
        /// Retrieves the size of a rectangular area into which
        /// a control can be fitted.
        /// </summary>
        public override Size GetPreferredSize(Size proposedSize)
        {
            return GetAutoSize();
        }

        /// <summary>
        /// Performs the work of setting the specified bounds of this control.
        /// </summary>
        protected override void SetBoundsCore(int x, int y, int width, int height,
                BoundsSpecified specified)
        {
            if (this.AutoSize && (specified & BoundsSpecified.Size) != 0)
            {
                Size size = GetAutoSize();

                width = size.Width;
                height = size.Height;
            }

            base.SetBoundsCore(x, y, width, height, specified);
        }
    }
}

using SkiaMonoSpaceRenderer;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SkiaMonoSpace.Control
{
    public class SkiaMonospace : ScrollableControl
    {
        SkiaMonospaceRenderTarget _renderTargetControl;

        public SkiaMonospace()
        {
            WidthInCharacters = DefaultWidthInCharacters;
            HeightInCharacters = DefaultHeightInCharacters;
            this.Font = new Font(DefaultFontname, DefaultTextsize);
            this.ForeColor = DefaultForeColor;
            this.BackColor = DefaultBackColor;

            // We can't use control.DesignerMode at this point, because that does not
            // work in a custom control (from that control's perspective, we're running).
            // So, we abuse the LicenseManager's UsageMode. More reliable in this context.
            bool designMode = (LicenseManager.UsageMode == LicenseUsageMode.Designtime);
            if (designMode)
            {
                return;
            }
            RecreateControl();
        }

        private void RecreateControl()
        {

            // This doesn't work in the Designer, because the Designer doesn't
            // pick up the skialib dll.
            var typeFace = SKTypeface.FromFamilyName(Font.Name,
                                              SKFontStyleWeight.Normal,
                                              SKFontStyleWidth.Normal,
                                              SKFontStyleSlant.Upright);

            _renderTargetControl = new SkiaMonospaceRenderTarget(typeFace, Font.Size,
                                        ForeColor.ToSKColor(), BackColor.ToSKColor(),
                                        WidthInCharacters, HeightInCharacters);
        }

        public new Color DefaultForeColor { get; } = Color.FloralWhite;
        public new Color DefaultBackColor { get; } = Color.DarkBlue;
        public string DefaultFontname { get; } = "Consolas";
        public float DefaultTextsize { get; } = 14;
        public int DefaultWidthInCharacters { get; } = 120;
        public int DefaultHeightInCharacters { get; } = 40;
        public int WidthInCharacters { get; set; }
        public int HeightInCharacters { get; set; }
        public Screenchar[] ScreenBuffer { get => _renderTargetControl._monoSpaceRenderer.ScreenBuffer; }
    }
}

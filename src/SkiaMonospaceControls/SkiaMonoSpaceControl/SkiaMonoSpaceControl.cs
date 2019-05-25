using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SkiaMonospace.Control
{
    public class SkiaMonospaceControl : ScrollableControl
    {
        SkiaMonospaceRenderTarget _renderTargetControl;

        public SkiaMonospaceControl()
        {
            WidthInCharacters = DefaultWidthInCharacters;
            HeightInCharacters = DefaultHeightInCharacters;
            this.Font = new Font(DefaultFontname, DefaultTextsize);
            this.ForeColor = DefaultForeColor;
            this.BackColor = DefaultBackColor;

            this.AutoScroll = true;

            // We can't use control.DesignerMode at this point, because that does not
            // work in a custom control (from that control's perspective, we're running).
            // So, we abuse the LicenseManager's UsageMode. More reliable in this context.
            bool designMode = (LicenseManager.UsageMode == LicenseUsageMode.Designtime);
            if (designMode)
            {
                return;
            }
            CreateRenderTarget();
        }

        private void CreateRenderTarget()
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
            Controls.Add(_renderTargetControl);
        }

        protected override void OnResize(EventArgs e)
        {
            if (ClientRectangle.Width > _renderTargetControl.Width)
            {
                _renderTargetControl.Left = (ClientRectangle.Width - _renderTargetControl.Width) / 2;
            }
            else
            {
                _renderTargetControl.Left = 0;
            }

            if (ClientRectangle.Height > _renderTargetControl.Height)
            {
                _renderTargetControl.Top = (ClientRectangle.Height - _renderTargetControl.Height) / 2;
            }
            else
            {
                _renderTargetControl.Top = 0;
            }
        }

        public void ClearScreen(char clearCharacter)
        {
            _renderTargetControl._monoSpaceRenderer.ClearScreen(clearCharacter);
            _renderTargetControl.Invalidate();
        }


        [Browsable(false)]
        public new Color DefaultForeColor { get; } = Color.FloralWhite;
        [Browsable(false)]
        public new Color DefaultBackColor { get; } = Color.DarkBlue;
        [Browsable(false)]
        public string DefaultFontname { get; } = "Consolas";
        [Browsable(false)]
        public float DefaultTextsize { get; } = 24;
        [Browsable(false)]
        public int DefaultWidthInCharacters { get; } = 120;
        [Browsable(false)]
        public int DefaultHeightInCharacters { get; } = 40;
        [Category("Layout")]
        public int WidthInCharacters { get; set; }
        [Category("Layout")]
        public int HeightInCharacters { get; set; }

        [Browsable(false)]
        public Screenchar[] ScreenBuffer { get => _renderTargetControl._monoSpaceRenderer.ScreenBuffer; }
    }
}

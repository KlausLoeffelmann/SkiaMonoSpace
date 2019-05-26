using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SkiaMonospace.Control
{
    public class SkiaMonospaceControl : ScrollableControl, ISupportInitialize
    {
        SkiaMonospaceRenderTarget _renderTargetControl;
        private bool _isDesignMode;
        private bool _renderTargetControlIsInstanciated;

        public SkiaMonospaceControl()
        {
            WidthInCharacters = DefaultWidthInCharacters;
            HeightInCharacters = DefaultHeightInCharacters;
            this.Font = new Font(DefaultFontname, DefaultTextsize);
            this.ForeColor = DefaultForeColor;
            this.BackColor = DefaultBackColor;

            this.AutoScroll = true;

            // This a) actually works instead of the DesignMode property and 
            // b) but only in the constructor, so we need to capture the result.
            _isDesignMode = LicenseManager.UsageMode == LicenseUsageMode.Designtime;
        }

        public void BeginInit()
        {
        }

        public void EndInit()
        {
            if (_isDesignMode)
            {
                return;
            }

            // We only want to create the rendertarget,
            // when we know how big the size of the textbuffer window will be,
            // so after WidthInCharcters and HeightsInCharacters have been set
            // by InitializeComponent of that component which uses this control.
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
            _renderTargetControlIsInstanciated = true;

            Controls.Add(_renderTargetControl);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (_isDesignMode)
                return;

            // OnResize is called at design time, so we need to avoid
            // loading the OpenTk assembly, since it is not compatible with
            // the WinForms designer and would fail to load.
            // We avoid loading it by avoiding referencing it,
            // so we use this ha...trick:
            if (!_renderTargetControlIsInstanciated)
            {
                return;
            }

            ResizeCore();
        }

        private void ResizeCore()
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
            if (_isDesignMode)
            {
                return;
            }

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

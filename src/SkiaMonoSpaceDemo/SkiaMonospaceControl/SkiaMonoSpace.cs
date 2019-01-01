using SkiaMonoSpaceRenderer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SkiaSharp.Views.Desktop;
using SkiaSharp;

namespace SkiaMonoSpace.Control
{
    public class SkiaMonospace : ScrollableControl
    {
        SkiaMonospaceRenderTarget _renderTargetControl;

        public new Color DefaultForeColor { get; } = Color.FloralWhite;
        public new Color DefaultBackColor { get; } = Color.DarkBlue;
        public string DefaultFontname { get; } = "Consolas";
        public float DefaultTextsize { get; } = 14;
        public int DefaultWidthInCharacters { get; } = 120;
        public int DefaultHeightInCharacters { get; } = 40;

        public SkiaMonospace()
        {
            WidthInCharacters = DefaultWidthInCharacters;
            HeightInCharacters = DefaultHeightInCharacters;
            this.Font = new Font(DefaultFontname, DefaultTextsize);
            this.ForeColor = DefaultForeColor;
            this.BackColor = DefaultBackColor;
            RecreateControl();
        }

        private void RecreateControl()
        {
            var typeFace = SKTypeface.FromFamilyName(Font.Name,
                                              SKFontStyleWeight.Normal,
                                              SKFontStyleWidth.Normal,
                                              SKFontStyleSlant.Upright);

            _renderTargetControl = new SkiaMonospaceRenderTarget(typeFace, Font.Size,
                                        ForeColor.ToSKColor(), BackColor.ToSKColor(),
                                        WidthInCharacters, HeightInCharacters);
        }

        public int WidthInCharacters { get; set; }

        public int HeightInCharacters { get; set; }

        public Screenchar[] ScreenBuffer { get => _renderTargetControl._monoSpaceRenderer.ScreenBuffer; }
    }
}


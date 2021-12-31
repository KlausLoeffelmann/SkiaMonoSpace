namespace SkiaMonoSpaceDemo
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.skiaMonospaceControl1 = new SkiaMonospace.Control.SkiaMonospaceControl();
            this.showSkiaSharpPlaygroundFormButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // skiaMonospaceControl1
            // 
            this.skiaMonospaceControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.skiaMonospaceControl1.AutoScroll = true;
            this.skiaMonospaceControl1.BackColor = System.Drawing.Color.DarkBlue;
            this.skiaMonospaceControl1.Font = new System.Drawing.Font("Consolas", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.skiaMonospaceControl1.ForeColor = System.Drawing.Color.FloralWhite;
            this.skiaMonospaceControl1.HeightInCharacters = 40;
            this.skiaMonospaceControl1.Location = new System.Drawing.Point(20, 22);
            this.skiaMonospaceControl1.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.skiaMonospaceControl1.Name = "skiaMonospaceControl1";
            this.skiaMonospaceControl1.Size = new System.Drawing.Size(1332, 931);
            this.skiaMonospaceControl1.TabIndex = 0;
            this.skiaMonospaceControl1.Text = "skiaMonospaceControl1";
            this.skiaMonospaceControl1.WidthInCharacters = 80;
            // 
            // showSkiaSharpPlaygroundFormButton
            // 
            this.showSkiaSharpPlaygroundFormButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.showSkiaSharpPlaygroundFormButton.Location = new System.Drawing.Point(1055, 962);
            this.showSkiaSharpPlaygroundFormButton.Name = "showSkiaSharpPlaygroundFormButton";
            this.showSkiaSharpPlaygroundFormButton.Size = new System.Drawing.Size(297, 47);
            this.showSkiaSharpPlaygroundFormButton.TabIndex = 1;
            this.showSkiaSharpPlaygroundFormButton.Text = "Show Skiasharp Playground";
            this.showSkiaSharpPlaygroundFormButton.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1372, 1024);
            this.Controls.Add(this.showSkiaSharpPlaygroundFormButton);
            this.Controls.Add(this.skiaMonospaceControl1);
            this.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.Name = "MainForm";
            this.Text = "Skia Monospace Demo";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private SkiaMonospace.Control.SkiaMonospaceControl skiaMonospaceControl1;
        private System.Windows.Forms.Button showSkiaSharpPlaygroundFormButton;
    }
}

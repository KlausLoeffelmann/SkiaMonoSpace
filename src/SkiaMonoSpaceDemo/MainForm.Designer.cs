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
            ((System.ComponentModel.ISupportInitialize)(this.skiaMonospaceControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // skiaMonospaceControl1
            // 
            this.skiaMonospaceControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.skiaMonospaceControl1.AutoScroll = true;
            this.skiaMonospaceControl1.BackColor = System.Drawing.Color.DarkBlue;
            this.skiaMonospaceControl1.Font = new System.Drawing.Font("Consolas", 24F);
            this.skiaMonospaceControl1.ForeColor = System.Drawing.Color.FloralWhite;
            this.skiaMonospaceControl1.HeightInCharacters = 40;
            this.skiaMonospaceControl1.Location = new System.Drawing.Point(12, 12);
            this.skiaMonospaceControl1.Name = "skiaMonospaceControl1";
            this.skiaMonospaceControl1.Size = new System.Drawing.Size(1245, 705);
            this.skiaMonospaceControl1.TabIndex = 0;
            this.skiaMonospaceControl1.Text = "skiaMonospaceControl1";
            this.skiaMonospaceControl1.WidthInCharacters = 120;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1269, 729);
            this.Controls.Add(this.skiaMonospaceControl1);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.skiaMonospaceControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private SkiaMonospace.Control.SkiaMonospaceControl skiaMonospaceControl1;
    }
}


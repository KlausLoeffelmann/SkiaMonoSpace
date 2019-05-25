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
            this.skiaMonospace1 = new SkiaMonospace.Control.SkiaMonospaceControl();
            this.SuspendLayout();
            // 
            // skiaMonospace1
            // 
            this.skiaMonospace1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.skiaMonospace1.BackColor = System.Drawing.Color.DarkBlue;
            this.skiaMonospace1.Font = new System.Drawing.Font("Consolas", 14F);
            this.skiaMonospace1.ForeColor = System.Drawing.Color.FloralWhite;
            this.skiaMonospace1.HeightInCharacters = 40;
            this.skiaMonospace1.Location = new System.Drawing.Point(13, 14);
            this.skiaMonospace1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.skiaMonospace1.Name = "skiaMonospace1";
            this.skiaMonospace1.Size = new System.Drawing.Size(1877, 1093);
            this.skiaMonospace1.TabIndex = 0;
            this.skiaMonospace1.Text = "skiaMonospace1";
            this.skiaMonospace1.WidthInCharacters = 120;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1903, 1121);
            this.Controls.Add(this.skiaMonospace1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private SkiaMonospace.Control.SkiaMonospaceControl skiaMonospace1;
    }
}


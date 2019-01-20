using System;
using System.Windows.Forms;

namespace SkiaMonoSpaceDemo
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var test = new SkiaMonospace.Control.SkiaMonospace();
            this.Controls.Add(test);

        }
    }
}

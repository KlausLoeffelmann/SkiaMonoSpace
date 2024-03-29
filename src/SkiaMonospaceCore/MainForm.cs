﻿using SkiaMonospaceCore;
using System;
using System.Windows.Forms;

namespace SkiaMonoSpaceDemo
{
    public partial class MainForm : Form
    {
        private Timer _timer;
        private int charCount = 65;

        public MainForm()
        {
            InitializeComponent();

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _timer = new Timer();
            _timer.Tick += _timer_Tick;
            _timer.Interval = 1000 / 25;
            _timer.Enabled = true;
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            skiaMonospaceControl1.ClearScreen((char)charCount++);
            Text = $"Skia MonoSpace [{skiaMonospaceControl1.LastFrameMs} ms.]";

            if (charCount > 365 + 24) charCount = 65;
        }

        private void RunSkiaPlayground_Click(object sender, EventArgs e)
        {
            SkiaSharpPlaygroundForm form = new();
            form.ShowDialog();
        }
    }
}

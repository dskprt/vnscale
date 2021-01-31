using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace vnscale {
    public partial class PlaybackForm : Form {

        VideoCapture capture;

        public PlaybackForm(VideoCapture capture) {
            InitializeComponent();

            this.capture = capture;

            Load += PlaybackForm_Load;
            FormClosed += PlaybackForm_FormClosed;
        }

        private void PlaybackForm_FormClosed(object sender, FormClosedEventArgs e) {
            timer1.Stop();
            if(capture != null) capture.Dispose();
            Application.Exit();
        }

        private void PlaybackForm_Load(object sender, EventArgs e) {
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e) {
            if(capture != null) {
                using (Image<Bgr, byte> frame = capture.QueryFrame().ToImage<Bgr, byte>()) {
                    pictureBox1.BackgroundImage = frame.ToBitmap();
                }
            }
        }
    }
}

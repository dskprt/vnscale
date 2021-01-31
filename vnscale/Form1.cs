using DirectShowLib;
using Emgu.CV;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace vnscale {
    public partial class Form1 : Form {

        DsDevice[] cameras;
        VideoCapture capture;

        public Form1() {
            InitializeComponent();

            Load += Form1_Load;
            FormClosed += Form1_FormClosed;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e) {
            if (capture != null) capture.Dispose();
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e) {
            cameras = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);

            for (int i = 0; i < cameras.Length; i++) {
                comboBox1.Items.Add("[" + i + "] " + cameras[i].Name + " {" + cameras[i].ClassID + "}");
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            if(comboBox1.SelectedItem != null) {
                capture = new VideoCapture(camIndex: comboBox1.SelectedIndex);

                PlaybackForm playback = new PlaybackForm(capture);
                playback.Show();

                Hide();
            }
        }
    }
}

using Anime4k.Algorithm;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Advanced;
using System.IO;

namespace vnscale {
    public partial class PlaybackForm : Form {

        VideoCapture capture;

        int iWidth;
        int iHeight;

        int sWidth;
        int sHeight;

        public PlaybackForm(VideoCapture capture, int iWidth, int iHeight, int sWidth, int sHeight) {
            InitializeComponent();

            this.capture = capture;

            this.iWidth = iWidth;
            this.iHeight = iHeight;

            this.sWidth = sWidth;
            this.sHeight = sHeight;

            Load += PlaybackForm_Load;
            FormClosed += PlaybackForm_FormClosed;

            this.ClientSize = new System.Drawing.Size(sWidth, sHeight);

            pictureBox1.Size = new System.Drawing.Size(sWidth, sHeight);
            pictureBox1.Location = new System.Drawing.Point(0, 0);
        }

        private void PlaybackForm_FormClosed(object sender, FormClosedEventArgs e) {
            timer1.Stop();
            if(capture != null) capture.Dispose();
            Application.Exit();
        }

        private void PlaybackForm_Load(object sender, EventArgs e) {
            ImageViewer viewer = new ImageViewer();
            Anime4KScaler scaler = new Anime4KScaler(Anime4KAlgorithmVersion.v10RC2);

            Application.Idle += (s, ea) => {
                if (capture != null) {
                    Mat frame = capture.QueryFrame();

                    Image<Rgba32> scaled = scaler.Scale(ToImageSharpImage<Rgba32>(frame.ToBitmap()), sHeight / iHeight);
                    viewer.Image = ToBitmap(scaled).ToImage<Bgr, byte>();
                }
            };
            viewer.ShowDialog();

            //timer1.Start();
        }

        public static System.Drawing.Bitmap ToBitmap<TPixel>(Image<TPixel> image) where TPixel : unmanaged, IPixel<TPixel> {
            using (var memoryStream = new MemoryStream()) {
                var imageEncoder = image.GetConfiguration().ImageFormatsManager.FindEncoder(PngFormat.Instance);
                image.Save(memoryStream, imageEncoder);

                memoryStream.Seek(0, SeekOrigin.Begin);

                return new System.Drawing.Bitmap(memoryStream);
            }
        }

        public static Image<TPixel> ToImageSharpImage<TPixel>(System.Drawing.Bitmap bitmap) where TPixel : unmanaged, IPixel<TPixel> {
            using (var memoryStream = new MemoryStream()) {
                bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);

                memoryStream.Seek(0, SeekOrigin.Begin);

                return SixLabors.ImageSharp.Image.Load<TPixel>(memoryStream);
            }
        }

        private void timer1_Tick(object sender, EventArgs e) {
            if(capture != null) {
                using (Image<Bgr, byte> frame = capture.QueryFrame().ToImage<Bgr, byte>()) {
                    pictureBox1.BackgroundImage = frame.AsBitmap();
                }
            }
        }
    }
}

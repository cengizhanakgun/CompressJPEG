using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Encoder = System.Drawing.Imaging.Encoder;

namespace CompressJPEG
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnResize_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JPEG Files|*.jpg;*.jpeg";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;

                FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    string outputPath = folderBrowserDialog.SelectedPath;
                    ResizeImage(filePath, outputPath);
                }
            }
        }

        private void ResizeImage(string filePath, string outputPath)
        {
            // Yeni boyutu al
            int newWidth = Convert.ToInt32(numericUpDownWidth.Value);
            int newHeight = Convert.ToInt32(numericUpDownHeight.Value);

            using (Image oldImage = Image.FromFile(filePath))
            {
                using (Bitmap newImage = new Bitmap(newWidth, newHeight))
                {
                    using (Graphics g = Graphics.FromImage(newImage))
                    {
                        g.DrawImage(oldImage, new Rectangle(0, 0, newWidth, newHeight));
                    }

                    // JPEG sıkıştırma kalitesi
                    EncoderParameter qualityParam = new EncoderParameter(Encoder.Quality, 100L);
                    ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");
                    if (jpegCodec != null)
                    {
                        EncoderParameters encoderParams = new EncoderParameters(1);
                        encoderParams.Param[0] = qualityParam;

                        // Yeni dosyayı belirtilen hedefe kaydet
                        string fileName = Path.GetFileName(filePath);
                        string outputPathWithFileName = Path.Combine(outputPath, fileName);
                        newImage.Save(outputPathWithFileName, jpegCodec, encoderParams);
                        MessageBox.Show("Resized and saved successfully.");
                    }
                }
            }
        }

        private ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.MimeType == mimeType)
                {
                    return codec;
                }
            }
            return null;
        }
    }


}

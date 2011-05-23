using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Drawing;
using System.IO;

namespace ZipFileViewer
{
    /// <summary>
    /// Interaction logic for ImageCanvas.xaml
    /// </summary>
    public partial class ImageCanvas : Window
    {
        public ImageCanvas()
        {
            InitializeComponent();
        }

        //TODO filter by mimetype
        public void ShowImage(MemoryStream stream) 
        {
            try
            {
                stream.Position = 0;
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();

                image1.Width = bitmapImage.Width;
                image1.Height = bitmapImage.Height;

                var screenInfo = GetScreenInfo();

                image1.Height = screenInfo.Height - 150;
                image1.Width = (image1.Height * bitmapImage.Width) / bitmapImage.Height;

                Height = image1.Height + 3;
                Width = image1.Width + 3;

                image1.Source = bitmapImage;
            }
            catch (NotSupportedException) 
            { }
        }

        public void ShowImage(BitmapImage bitmapImage)
        {
            image1.Width = bitmapImage.Width;
            image1.Height = bitmapImage.Height;

            var screenInfo = GetScreenInfo();

            image1.Height = screenInfo.Height - 150;
            image1.Width = (image1.Height * bitmapImage.Width) / bitmapImage.Height;

            Height = image1.Height + 3;
            Width = image1.Width + 3;

            image1.Source = bitmapImage;
        }

        private System.Drawing.Size GetScreenInfo() 
        {
            var width = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width;
            var height = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height;
            var size = new System.Drawing.Size(width, height);
            return size;
        }
    }
}

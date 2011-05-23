using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ionic.Zip;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace ZipFileViewer
{
    class Controller
    {
        private string fileName;

        public void SetFileName(string path) 
        {
            fileName = path;
        }

        public IEnumerable<string> GetList() 
        {
            List<string> names;
            using(var zipFile = ZipFile.Read(fileName))
            {
                names = zipFile.Select(zip => zip.FileName).ToList();
            }

            return names;
        }

        public void ShowImageBrowser(IEnumerable<string> filesNames)
        {
            var browser = InitImageBrowser();
            PopulateImageBrowser(browser, filesNames);
            browser.Show();
        }

        private void PopulateImageBrowser(ImageBrowser browser, IEnumerable<string> filesNames)
        {
            for (int i = 0; i < 15; i++) 
            {
                var bitmapImage = LoadBitmapImage(filesNames.ToArray()[i]);
                var imageControl = new System.Windows.Controls.Image();
                imageControl.Opacity = 0.5;
                imageControl.MouseDown += new System.Windows.Input.MouseButtonEventHandler(ImageControlMouseDown);
                imageControl.MouseEnter += new System.Windows.Input.MouseEventHandler(imageControl_MouseEnter);
                imageControl.MouseLeave += new System.Windows.Input.MouseEventHandler(imageControl_MouseLeave);
                imageControl.Source = bitmapImage;
                browser.Add(imageControl);
 
            }
                //foreach (var file in filesNames)
                //{
                //    var bitmapImage = LoadBitmapImage(file);
                //    var imageControl = new System.Windows.Controls.Image();
                //    imageControl.Source = bitmapImage;
                //    browser.Add(imageControl);
                //}
        }

        void imageControl_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var imageControl = sender as System.Windows.Controls.Image;
            imageControl.Opacity = 0.5;
        }

        void imageControl_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var imageControl = sender as System.Windows.Controls.Image;
            imageControl.Opacity = 1;
        }

        void ImageControlMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var imageControl = sender as System.Windows.Controls.Image;
            ShowImage(imageControl.Source as BitmapImage);
        }

        private BitmapImage LoadBitmapImage(string file)
        {            
            var bitmapImage = new BitmapImage();
            var image = OpenImage(file);
            image.Position = 0;
            try
            {   
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = image;
                bitmapImage.EndInit();
            }
            catch (NotSupportedException)
            { }

            return bitmapImage;
        }

        private ImageBrowser InitImageBrowser()
        {
            Size imageBrowserSize = GetImageBrowserSize();
            Point imageBrowserLocation = GetImageBrowserLocation();
            var browser = new ImageBrowser();
            browser.Width = imageBrowserSize.Width;
            browser.Height = imageBrowserSize.Height;
            browser.Left = imageBrowserLocation.X;
            browser.Top = imageBrowserLocation.Y;
            return browser;
        }

        private Size GetImageBrowserSize()
        {
            var size = new Size();
            size.Width = (int)System.Windows.SystemParameters.WorkArea.Width;
            size.Height = (int)System.Windows.SystemParameters.WorkArea.Height / 4;

            return size;
        }

        private Point GetImageBrowserLocation()
        {
            var location = new Point();
            location.Y = 0;
            location.X = 0;
            return location;
        }

        public MemoryStream OpenImage(string path) 
        {
            var memoryStream = new MemoryStream();
            using (var zipFile = ZipFile.Read(fileName))
            {
                var file = zipFile[path];
                file.Extract(memoryStream);
            }

            return memoryStream;            
        }

        public void ShowImage(MemoryStream memoryStream)
        {
            var viewer = new ImageCanvas();
            viewer.Title = fileName;
            viewer.ShowImage(memoryStream);
            viewer.Show();
        }

        public void ShowImage(BitmapImage bitmap)
        {
            var viewer = new ImageCanvas();
            viewer.ShowImage(bitmap);
            viewer.Show();
        }
    }
}

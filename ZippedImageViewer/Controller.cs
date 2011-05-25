using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;
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

        private async void PopulateImageBrowser(ImageBrowser browser, IEnumerable<string> filesNames)
        {
            var bitmapImages = await LoadThumbnailBitmapImage(filesNames);
            var observableImages = bitmapImages.ToObservable();
            observableImages.Subscribe(keypair =>
            {
                Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, new Action<ImageBrowser, BitmapImage, string>(AddImage), browser, keypair.Value, keypair.Key);
            });
        }

        private void AddImage(ImageBrowser browser, BitmapImage bitmapImage, string file)
        {
            var imageControl = new System.Windows.Controls.Image
            {
                Opacity = 0.5,
                Margin = new System.Windows.Thickness(10, 10, 10, 10),
                Source = bitmapImage,
                Tag = file
            };

            imageControl.MouseDown += ImageControlMouseDown;
            imageControl.MouseEnter += ImageControlMouseEnter;
            imageControl.MouseLeave += ImageControlMouseLeave;
            browser.Add(imageControl);
        }

        private void ImageControlMouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var imageControl = (System.Windows.Controls.Image)sender;
            imageControl.Margin = new System.Windows.Thickness(10, 10, 10, 10);
            imageControl.Opacity = 0.5;
        }

        private void ImageControlMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var imageControl = (System.Windows.Controls.Image)sender;
            imageControl.Margin = new System.Windows.Thickness(0, 0, 0, 0);
            imageControl.Opacity = 1;
        }

        void ImageControlMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var imageControl = (System.Windows.Controls.Image)sender;
            ShowImage(OpenImage(imageControl.Tag as string));
        }

        private async Task<Dictionary<string,BitmapImage>> LoadThumbnailBitmapImage(IEnumerable<string> files)
        {
            var images = new Dictionary<string,BitmapImage>();
            foreach (var file in files)
            {
                var bitmapImage = new BitmapImage();
                var image = await TaskEx.Run(()=>OpenImage(file));
                image.Position = 0;
                try
                {
                    bitmapImage.BeginInit();
                    bitmapImage.DecodePixelWidth = 300;
                    bitmapImage.CacheOption = BitmapCacheOption.None;
                    bitmapImage.StreamSource = image;
                    bitmapImage.EndInit();
                    images.Add(file, bitmapImage);
                }
                catch (NotSupportedException)
                {

                }
            }

            return images;
        }

        private ImageBrowser InitImageBrowser()
        {
            var imageBrowserSize = GetImageBrowserSize();
            var imageBrowserLocation = GetImageBrowserLocation();
            var browser = new ImageBrowser
                              {
                                  Width = imageBrowserSize.Width,
                                  Height = imageBrowserSize.Height,
                                  Left = imageBrowserLocation.X,
                                  Top = imageBrowserLocation.Y
                              };
            return browser;
        }

        private static Size GetImageBrowserSize()
        {
            var size = new Size
                           {
                               Width = (int) System.Windows.SystemParameters.WorkArea.Width,
                               Height = (int) System.Windows.SystemParameters.WorkArea.Height/4
                           };

            return size;
        }

        private static Point GetImageBrowserLocation()
        {
            var location = new Point {Y = 0, X = 0};
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
            var viewer = new ImageCanvas {Title = fileName};
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ionic.Zip;
using System.Drawing;
using System.IO;

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
            Size imageBrowserSize = GetImageBrowserSize();
            Point imageBrowserLocation = GetImageBrowserLocation();
            var browser = new ImageBrowser();
            browser.Width = imageBrowserSize.Width;
            browser.Height = imageBrowserSize.Height;
            browser.Left = imageBrowserLocation.X;
            browser.Top = imageBrowserLocation.Y;
            browser.Show();
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

        public void OpenImage(string path) 
        {
            var memoryStream = new MemoryStream();
            using (var zipFile = ZipFile.Read(fileName))
            {
                var file = zipFile[path];
                file.Extract(memoryStream);
            }

            var viewer = new ImageCanvas();
            viewer.Title = fileName;
            viewer.ShowImage(memoryStream);
            viewer.Show();
        }
    }
}

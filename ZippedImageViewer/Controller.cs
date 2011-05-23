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
            //var browser = new ImageBrowser();
            //browser.Show();
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

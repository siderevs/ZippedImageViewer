using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZipFileViewer
{
    class Startup
    {
        [STAThread]
        static void Main()
        {
            App app = new App();
            app.MainWindow = new MainWindow();
            app.MainWindow.Show();
            app.Run();
        }
    }
}

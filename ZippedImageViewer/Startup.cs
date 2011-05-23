using System;

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

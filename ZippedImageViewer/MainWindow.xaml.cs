using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace ZipFileViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private Controller controller;

        public MainWindow()
        {
            InitializeComponent();
            controller = new Controller();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            var file = new OpenFileDialog();
            var result = file.ShowDialog();
            if (result != true) return;

            controller.SetFileName(file.FileName);
            var list = controller.GetList();

            listView1.ItemsSource = list;
            controller.ShowImageBrowser(list);
        }

        private void listView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var fileName = ((ListView)sender).SelectedItem as string;
            if(string.IsNullOrEmpty(fileName)) return;

            var image = controller.OpenImage(fileName);
            controller.ShowImage(image);
        }
    }
}

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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace ZipFileViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
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
            Nullable<bool> result = file.ShowDialog();
            if (result == true) 
            {
                controller.SetFileName(file.FileName);
                var list = controller.GetList();

                listView1.ItemsSource = list;
                controller.ShowImageBrowser(list);
            }
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

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ZipFileViewer
{
	/// <summary>
	/// Interaction logic for ImageBrowser.xaml
	/// </summary>
	public partial class ImageBrowser : Window
	{
		public ImageBrowser()
		{
			this.InitializeComponent();
			
			// Insert code required on object creation below this point.
		}

        public void Add(Image image) 
        {
            stackPanel1.Children.Add(image);
        }
	}
}
using System.Windows;
using System.Windows.Controls;

namespace ZipFileViewer
{
	/// <summary>
	/// Interaction logic for ImageBrowser.xaml
	/// </summary>
	public partial class ImageBrowser
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
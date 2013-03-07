using System.Windows.Controls;

namespace ModuleA
{
	/// <summary>
	/// Interaction logic for ContentView.xaml
	/// </summary>
	public partial class ContentView : UserControl, IContentView
	{
		public ContentView()
		{
			InitializeComponent();
		}

		public FirstPrismApp.Infrastructure.IViewModel ViewModel
		{
			get
			{
				return (IContentViewViewModel)DataContext;
			}
			set
			{
				DataContext = value;
			}
		}
	}
}
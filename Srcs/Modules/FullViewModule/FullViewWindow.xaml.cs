using FirstPrismApp.Infrastructure;
using FirstPrismApp.Infrastructure.Base;
using System.Windows;

namespace FullViewModule
{
	/// <summary>
	/// Interaction logic for FullView.xaml
	/// </summary>
	public partial class FullViewWindow : Window, IFullView, IWindow
	{
		public FullViewWindow()
		{
			InitializeComponent();
		}

		public IViewModel ViewModel
		{
			get
			{
				return DataContext as IViewModel;
			}
			set
			{
				DataContext = value;
			}
		}
	}
}
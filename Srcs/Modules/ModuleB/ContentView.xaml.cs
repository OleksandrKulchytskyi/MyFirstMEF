using FirstPrismApp.Infrastructure;
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

namespace ModuleB
{
	/// <summary>
	/// Interaction logic for ContentView.xaml
	/// </summary>
	public partial class ContentView : UserControl,IView
	{
		public ContentView(IContentViewViewModel viewModel)
		{
			InitializeComponent();
			ViewModel = viewModel;
		}

		public IViewModel ViewModel
		{
			get
			{
				return (IViewModel)DataContext;
			}
			set
			{
				DataContext = value;
			}
		}
	}
}

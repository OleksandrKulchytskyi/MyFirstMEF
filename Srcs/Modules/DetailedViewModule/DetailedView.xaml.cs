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

namespace DetailedViewModule
{
	/// <summary>
	/// Interaction logic for DetailedView.xaml
	/// </summary>
	public partial class DetailedView : UserControl, IDetailedView
	{
		public DetailedView()
		{
			InitializeComponent();
		}

		public FirstPrismApp.Infrastructure.IViewModel ViewModel
		{
			get
			{
				return (IDetailedViewViewModel)DataContext;
			}
			set
			{
				DataContext = value;
			}
		}
	}
}

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

namespace ItemsViewModule
{
	/// <summary>
	/// Interaction logic for ItemsView.xaml
	/// </summary>
	public partial class ItemsView : UserControl, IItemsView
	{
		public ItemsView()
		{
			InitializeComponent();
		}

		public Core.Infrastructure.IViewModel ViewModel
		{
			get
			{
				return (IItemsViewViewModel)DataContext;
			}
			set
			{
				DataContext = value;
			}
		}
	}
}

using System.Windows.Controls;

namespace ToolbarModule
{
	public partial class ToolbarView : UserControl, IToolbarView
	{
		public ToolbarView()
		{
			InitializeComponent();
		}

		public FirstPrismApp.Infrastructure.IViewModel ViewModel
		{
			get
			{
				return (IToolbarViewViewModel)DataContext;
			}
			set
			{
				DataContext = value;
			}
		}
	}
}
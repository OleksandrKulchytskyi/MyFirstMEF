using System.Windows.Controls;

namespace ToolbarModule
{
	public partial class ToolbarView : UserControl, IToolbarView
	{
		public ToolbarView()
		{
			InitializeComponent();
		}

		public Core.Infrastructure.IViewModel ViewModel
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
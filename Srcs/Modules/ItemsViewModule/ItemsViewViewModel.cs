using Business.Common;
using FirstPrismApp.Infrastructure;
using System.Collections.ObjectModel;

namespace ItemsViewModule
{
	public class ItemsViewViewModel : ViewModelBase, IItemsViewViewModel
	{
		public ItemsViewViewModel(IItemsView view)
			: base(view)
		{
		}

		public ObservableCollection<LogItem> Entries { get; set; }
	}
}
using Business.Common;
using FirstPrismApp.Infrastructure;
using System.Collections.ObjectModel;

namespace ItemsViewModule
{
	public interface IItemsViewViewModel : IViewModel
	{
		ObservableCollection<LogItem> Entries { get; set; }
	}
}
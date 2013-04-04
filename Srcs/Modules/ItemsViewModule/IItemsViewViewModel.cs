using Business.Common;
using Core.Infrastructure;
using System.Collections.ObjectModel;

namespace ItemsViewModule
{
	public interface IItemsViewViewModel : IViewModel
	{
		ObservableCollection<LogItem> Entries { get; set; }
		bool IsBusy { get; set; }
	}
}
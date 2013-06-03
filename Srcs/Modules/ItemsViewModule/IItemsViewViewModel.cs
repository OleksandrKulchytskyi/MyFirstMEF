using Business.Common;
using Core.Infrastructure;
using Core.Infrastructure.Helpers;
using System.Collections.ObjectModel;

namespace ItemsViewModule
{
	public interface IItemsViewViewModel : IViewModel
	{
		VirtualizingCollection<LogItem> VirtColletion { get; set; }

		//ObservableCollection<LogItem> Entries { get; set; }

		bool IsBusy { get; set; }
	}
}
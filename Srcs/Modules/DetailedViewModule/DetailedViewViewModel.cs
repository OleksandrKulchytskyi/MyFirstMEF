using Business.Common;
using FirstPrismApp.Infrastructure;

namespace DetailedViewModule
{
	public class DetailedViewViewModel : ViewModelBase, IDetailedViewViewModel
	{
		public DetailedViewViewModel(IDetailedView view)
			: base(view)
		{
		}

		public  LogEntryDescription Entry { get; set; }
	}
}
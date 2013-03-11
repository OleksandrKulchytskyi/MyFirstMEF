using Business.Common;
using FirstPrismApp.Infrastructure;

namespace DetailedViewModule
{
	public interface IDetailedViewViewModel : IViewModel
	{
		LogEntryDescription Entry { get; set; }
	}
}
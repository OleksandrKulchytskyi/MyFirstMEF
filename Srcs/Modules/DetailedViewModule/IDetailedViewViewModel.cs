using Business.Common;
using Core.Infrastructure;

namespace DetailedViewModule
{
	public interface IDetailedViewViewModel : IViewModel
	{
		LogEntryDescription Entry { get; set; }
	}
}
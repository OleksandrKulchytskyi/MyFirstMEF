using Business.Common;
using System.Collections.Generic;

namespace Core.Infrastructure.Helpers
{
	public sealed class LogItemsPool : Pool<LogItem>
	{
		public LogItemsPool(int initialCount)
			: base(500)
		{
			TryAllocatePush(initialCount);
		}

		public LogItemsPool(int initialCount, int maxCapacity)
			: base(maxCapacity)
		{
			TryAllocatePush(initialCount);
		}

		public void ReleaseBulk(IEnumerable<PoolSlot<LogItem>> items)
		{
			foreach (var item in items)
			{
				Release(item);
			}
		}

		protected override LogItem ObjectConstructor()
		{
			return new LogItem();
		}

		protected override void CleanUp(LogItem @object)
		{
			base.CleanUp(@object);
			@object.LineNumber = 0;
			@object.Severity = string.Empty;
		}
	}
}
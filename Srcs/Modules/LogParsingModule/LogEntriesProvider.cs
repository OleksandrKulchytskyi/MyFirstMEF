using Business.Common;
using Core.Infrastructure.Base;
using Core.Infrastructure.Helpers;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.IO;

namespace LogParsingModule
{
	public class LogEntriesProvider : IItemsProvider<LogItem>
	{
		private string _fPath;
		private int _count;
		private IList<int> _items;
		private GenericWeakReference<LogItemsPool> _poolWeak;

		[InjectionConstructor()]
		public LogEntriesProvider(IUnityContainer container)
		{
			_count = -1;
			if (container != null)
				_poolWeak = new GenericWeakReference<LogItemsPool>(container.Resolve<LogItemsPool>());
		}

		public LogEntriesProvider(string fileName, IUnityContainer container)
			: this(container)
		{
			_fPath = fileName;
		}

		public int FetchCount()
		{
			if (_count == -1)
				FetchInternal(out _items);
			return _count;
		}

		public void SetSource(string filePath)
		{
			_fPath = filePath;
		}

		private void FetchInternal(out IList<int> items)
		{
			if (!File.Exists(_fPath))
				throw new FileNotFoundException("File wasn't found.", _fPath);

			items = new List<int>();
			using (StreamReader sr = new StreamReader(_fPath, true))
			{
				string line = null;
				int lineNumber = 0;
				Severity severity = Severity.None;
				bool found = false;
				_count = 0;
				while ((line = sr.ReadLine()) != null)
				{
					lineNumber++;

					if (string.IsNullOrEmpty(line)) continue;

					if (found && !LogParser.IsMessageBegin(line, out severity)) continue;

					else if (found && LogParser.IsMessageBegin(line, out severity))
						found = false;

					found = LogParser.IsMessageBegin(line, out severity);
					if (found)
					{
						items.Add(lineNumber);
						_count++;
					}
				}//end while
			}//end using scope
		}

		public IList<PoolSlot<LogItem>> FetchRange(int startIndex, int count)
		{
			return FetchChunk(startIndex, count);
		}

		private IList<PoolSlot<LogItem>> FetchChunk(int startIndx, int count)
		{
			if (_count == -1)//difensive check in case when FetchCount method wasn't invoked
				FetchInternal(out _items);

			if (startIndx < 0)
				throw new IndexOutOfRangeException("startIndx parameter must be equals or great than zero.");

			if (!File.Exists(_fPath))
				throw new FileNotFoundException("File wasn't found.", _fPath);

			IList<PoolSlot<LogItem>> slots = null;
			if (_poolWeak != null && _poolWeak.IsAlive)
				slots = _poolWeak.Target.TakeSlots(count);

			using (StreamReader sr = new StreamReader(_fPath, true))
			{
				string line = null;
				int lineNumber = 0;
				Severity severity = Severity.None;
				bool found = false;
				int retrieved = 0;
				int atPos = _items[startIndx];

				while ((lineNumber != (atPos == 0 ? atPos : atPos - 1)) && ((line = sr.ReadLine()) != null))//skip first lines if startIndex > 0
					lineNumber++;

				while ((line = sr.ReadLine()) != null)
				{
					lineNumber++;

					if (string.IsNullOrEmpty(line)) continue;

					if (found && !LogParser.IsMessageBegin(line, out severity)) continue;

					else if (found && LogParser.IsMessageBegin(line, out severity))
						found = false;

					found = LogParser.IsMessageBegin(line, out severity);
					if (found)
					{
						slots[retrieved].Object.Severity = severity.ToString();
						slots[retrieved].Object.LineNumber = lineNumber;
						slots[retrieved].Object.Time = LogParser.ExtractTime(line);
						retrieved++;
					}
					if (count == retrieved)
						break;
				}//end while
			}//end using sr
			return slots;
		}
	}
}
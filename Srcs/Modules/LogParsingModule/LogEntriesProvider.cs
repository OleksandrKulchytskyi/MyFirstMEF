using Business.Common;
using Core.Infrastructure.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LogParsingModule
{
	public class LogEntriesProvider : IItemsProvider<LogItem>
	{
		private string _fPath;
		private int _count;
		private IList<int> _items;

		[Microsoft.Practices.Unity.InjectionConstructor()]
		public LogEntriesProvider()
		{
			_count = -1;
		}

		public LogEntriesProvider(string fileName)
			: this()
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
				}
			}//end using scope
		}

		public IList<LogItem> FetchRange(int startIndex, int count)
		{
			return FetchChunk(startIndex, count);
		}

		private IList<LogItem> FetchChunk(int startIndx, int count)
		{
			if (_count == -1)//difence check
				FetchInternal(out _items);

			if (startIndx < 0)
				throw new IndexOutOfRangeException("startIndx parameter must be equals or great than zero.");

			if (!File.Exists(_fPath))
				throw new FileNotFoundException("File wasn't found.", _fPath);

			List<LogItem> entries = new List<LogItem>();
			LogItem item = null;

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
						item = new LogItem();
						item.Severity = severity.ToString();
						item.LineNumber = lineNumber;
						item.Time = LogParser.ExtractTime(line);
						entries.Add(item);
						retrieved++;
					}
					if (count == retrieved)
						break;
				}
			}//end using
			return entries;
		}
	}
}

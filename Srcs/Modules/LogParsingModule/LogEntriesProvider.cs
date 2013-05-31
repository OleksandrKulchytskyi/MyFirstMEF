﻿using Business.Common;
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

		public LogEntriesProvider(string fileName)
		{
			_fPath = fileName;
			_count = -1;
		}

		public int FetchCount()
		{
			if (_count == -1)
			{
				FetchInternal(out _items);
			}

			return _count;
		}

		private void FetchInternal(out IList<int> items)
		{
			if (!File.Exists(_fPath))
				throw new FileNotFoundException("File wasn't found.", _fPath);

			items = new List<int>();
			using (StreamReader sr = new StreamReader(_fPath, true))
			{
				string line = null;
				int lineNumber = 1;
				Severity severity = Severity.None;
				bool found = false;
				_count = 0;
				while ((line = sr.ReadLine()) != null)
				{
					lineNumber++;

					if (string.IsNullOrEmpty(line))
						continue;

					if (found && !LogParser.IsMessageBegin(line, out severity))
						continue;

					else if (found && LogParser.IsMessageBegin(line, out severity))
						found = false; severity = Severity.None;

					found = LogParser.IsMessageBegin(line, out severity);
					if (found)
					{
						items.Add(lineNumber);
						_count++;
					}
				}
			}
		}

		public IList<LogItem> FetchRange(int startIndex, int count)
		{
			return FetchChunk(startIndex, count);
		}

		private IList<LogItem> FetchChunk(int startIndx, int count)
		{
			if (!File.Exists(_fPath))
				throw new FileNotFoundException("File wasn't found.", _fPath);

			List<LogItem> entries = new List<LogItem>();
			LogItem item = null;

			using (StreamReader sr = new StreamReader(_fPath, true))
			{
				string line = null;
				int lineNumber = 1;
				Severity severity = Severity.None;
				bool found = false;
				_count = 0;
				while ((line = sr.ReadLine()) != null)
				{
					lineNumber++;

					if (string.IsNullOrEmpty(line))
						continue;

					if (found && !LogParser.IsMessageBegin(line, out severity))
						continue;

					else if (found && LogParser.IsMessageBegin(line, out severity))
						found = false; severity = Severity.None;

					found = LogParser.IsMessageBegin(line, out severity);
					if (found)
					{
						item = new LogItem();
						item.Severity = severity.ToString();
						item.LineNumber = lineNumber;
						item.Time = LogParser.ExtractTime(line);
						entries.Add(item);
						_count++;
					}
				}
			}
			return entries;
		}
	}
}

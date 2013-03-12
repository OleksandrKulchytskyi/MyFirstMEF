using Business.Common;
using System.Collections.Generic;
using System.IO;

namespace LogParsingModule
{
	public class LogParser : FirstPrismApp.Infrastructure.Services.IParsingService
	{
		public static IList<string> Msgs = new List<string>(4) { "INFO", "WARN", "ERROR", "FATAL" };

		public IList<LogItem> ParseLog(string filePath)
		{
			if (!File.Exists(filePath))
				throw new FileNotFoundException("File wasn't found.", filePath);

			IList<LogItem> entries = new List<LogItem>();
			using (StreamReader sr = new StreamReader(filePath, true))
			{
				string line = null;
				int lineNumber = 1;
				LogItem item = null;
				Severity severity = Severity.None;
				bool found = false;

				while ((line = sr.ReadLine()) != null)
				{
					if (string.IsNullOrEmpty(line))
					{
						lineNumber++;
						continue;
					}

					if (found && !IsMessageBegin(line, out severity))
					{
						lineNumber++;
						continue;
					}
					else if (found && IsMessageBegin(line, out severity))
					{
						found = false; severity = Severity.None; item = null;
					}

					found = IsMessageBegin(line, out severity);
					if (found)
					{
						item = new LogItem();
						item.Severity = severity.ToString();
						item.LineNumber = lineNumber;
						entries.Add(item);
					}
					lineNumber++;
				}
			}

			return entries;
		}

		internal static bool IsMessageBegin(string line, out Severity sever)
		{
			bool result = false;
			sever = Severity.None;

			foreach (string msg in LogParser.Msgs)
			{
				if (line.StartsWith(msg, System.StringComparison.OrdinalIgnoreCase))
				{
					result = true;
					sever = SeverityHelper.Mapping[msg];
					break;
				}
			}
			return result;
		}
	}
}
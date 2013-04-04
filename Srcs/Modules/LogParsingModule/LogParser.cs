using Business.Common;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LogParsingModule
{
	public class LogParser : Core.Infrastructure.Services.IParsingService
	{
		private const string _oneSpace = " ";
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
						item.Time = ExtractTime(line);
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

		internal static System.DateTime ExtractTime(string line)
		{
			if (string.IsNullOrEmpty(line))
				return new System.DateTime();

			int idx1 = line.IndexOf(_oneSpace);
			int idx2 = line.IndexOf(_oneSpace, (idx1 + 2));
			int idx3 = line.IndexOf(",", (idx2 + 2));

			if (idx1 > 0 && idx2 > 0 && idx3 > 0)
			{
				StringBuilder sb = new StringBuilder();
				sb.Append(line.Substring(idx1, idx2 - idx1));
				sb.Append(line.Substring(idx2, idx3 - idx2));
				System.DateTime time;
				if (System.DateTime.TryParse(sb.ToString(), out time))
					return time;

				sb = null;
			}
			return new System.DateTime();
		}
	}
}
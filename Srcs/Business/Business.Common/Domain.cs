using System;

namespace Business.Common
{
	public class LogItem
	{
		public int LineNumber { get; set; }

		public DateTime Time { get; set; }

		public string Severity { get; set; }
	}

	public class LogEntryDescription
	{
		public string Severity { get; set; }

		public DateTime Time { get; set; }

		public string Content { get; set; }
	}
}
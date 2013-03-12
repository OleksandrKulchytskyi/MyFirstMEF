using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogParsingModule
{
	public enum Severity
	{
		None = 0,
		Info = 1,
		Warn = 2,
		Error,
		Fatal
	}

	public sealed class SeverityHelper
	{
		public static IDictionary<string, Severity> _items;

		static SeverityHelper()
		{
			_items = new Dictionary<string, Severity>(StringComparer.OrdinalIgnoreCase);
			_items.Add("Undefined", Severity.None);
			_items.Add(LogParser.Msgs[0], Severity.Info);
			_items.Add(LogParser.Msgs[1], Severity.Warn);
			_items.Add(LogParser.Msgs[2], Severity.Error);
			_items.Add(LogParser.Msgs[3], Severity.Fatal);
		}

		public static IDictionary<string, Severity> Mapping
		{
			get { return _items; }
		}
	}
}

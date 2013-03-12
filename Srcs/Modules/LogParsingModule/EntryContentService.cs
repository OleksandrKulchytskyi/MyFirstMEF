using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LogParsingModule
{
	public sealed class EntryContentService : FirstPrismApp.Infrastructure.Services.IEntryContentService
	{
		public string GetErrorContentForLine(string file, int fromLine)
		{
			if (!File.Exists(file))
				throw new FileNotFoundException("File wasn't found.", file);
			string result = null;
			using (StreamReader sr = new StreamReader(file, true))
			{
				string line = sr.ReadLine(); ;
				int lineNumber = 1;
				Severity severity = Severity.None;
				bool found = false;
				StringBuilder contentBuilder = new StringBuilder();

				while (lineNumber != fromLine && line != null)
				{
					line = sr.ReadLine();
					lineNumber++;
				}

				found = LogParser.IsMessageBegin(line, out severity);
				int indx = line.LastIndexOf('-');
				contentBuilder.AppendLine(line.Substring(indx));

				while ((line = sr.ReadLine()) != null)
				{
					if (found && !LogParser.IsMessageBegin(line, out severity))
					{
						contentBuilder.AppendLine(line);
						continue;
					}
					else if (found && LogParser.IsMessageBegin(line, out severity))
					{
						result = contentBuilder.ToString();
						contentBuilder.Clear();
						break;
					}
				}
			}

			return result;
		}
	}
}

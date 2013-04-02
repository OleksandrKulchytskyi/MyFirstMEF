using FirstPrismApp.Infrastructure.Base;
using FirstPrismApp.Infrastructure.Services;
using Microsoft.Practices.Unity;
using System.IO;
using System.Text;

namespace LogParsingModule
{
	public sealed class EntryContentService : IEntryContentService
	{
		private IUnityContainer _container = null;

		public EntryContentService(IUnityContainer container)
		{
			_container = container;
		}

		public string GetErrorContentForLine(string file, int fromLine)
		{
			if (!File.Exists(file))
			{
				_container.Resolve<ILogger>().Log(LogSeverity.Warn, string.Format("File {0} is not exists", file), null);
				throw new FileNotFoundException("File wasn't found.", file);
			}

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
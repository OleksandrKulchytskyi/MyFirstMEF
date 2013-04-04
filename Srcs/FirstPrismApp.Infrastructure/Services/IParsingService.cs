using Business.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Infrastructure.Services
{
	public interface IParsingService
	{
		IList<LogItem> ParseLog(string filePath);
	}
}

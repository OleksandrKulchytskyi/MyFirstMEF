using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FirstPrismApp.Infrastructure.Base
{
	public enum LogSeverity
	{
		Info = 0,
		Warn,
		Error,
		Fatal
	}

	public interface ILogger
	{
		void Log(LogSeverity severity, string payload, Exception ex);
	}
}

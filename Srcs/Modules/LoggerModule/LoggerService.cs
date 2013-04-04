using Core.Infrastructure.Base;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoggerModule
{
	public class LoggerService : ILogger
	{
		private readonly ILog _logger = null;

		public LoggerService()
		{
			string path = System.IO.Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
			log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.DirectoryInfo(path).EnumerateFiles().FirstOrDefault(x => x.Extension.IndexOf("config") != -1));
			_logger = log4net.LogManager.GetLogger(typeof(LoggerService));
		}

		public void Log(LogSeverity severity, string payload, Exception ex)
		{
			switch (severity)
			{
				case LogSeverity.Info:
					_logger.Info(payload);
					break;

				case LogSeverity.Warn:
					_logger.Warn(payload);
					break;

				case LogSeverity.Error:
					_logger.Error(payload,ex);
					break;

				case LogSeverity.Fatal:
					_logger.Fatal(payload, ex);
					break;

				default:
					break;
			}
		}
	}
}

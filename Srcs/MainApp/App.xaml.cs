using Core.Infrastructure.Base;
using Microsoft.Practices.Unity;
using System.Windows;

namespace MainApp
{
	public partial class App : Application
	{
		private Microsoft.Practices.Unity.IUnityContainer _container;

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			this.DispatcherUnhandledException += App_DispatcherUnhandledException;
			Bootstrapper boot = new Bootstrapper();
			_container = boot.Container;
			boot.Run();
		}

		private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
		{
			if (_container == null)
				return;

			ILogger logger = _container.Resolve<ILogger>();
			if (null != logger)
				logger.Log(LogSeverity.Fatal, e.Exception.Message, e.Exception);
		}
	}
}
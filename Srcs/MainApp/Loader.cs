using FirstPrismApp.Infrastructure;
using FirstPrismApp.Infrastructure.Services;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MainApp
{
	class Loader
	{
		private IUnityContainer _container;

		public Loader(IUnityContainer container)
		{
			_container = container;
			InitializeCoreServices();
			//LoadTheme();
			LoadCommands();
			//LoadMenus();
			//LoadToolbar();
		}

		private void InitializeCoreServices()
		{
			_container.RegisterType<ICommandManager, CommandManager>(new Microsoft.Practices.Unity.ContainerControlledLifetimeManager());
		}

		private void LoadCommands()
		{
			ICommandManager manager = _container.Resolve<ICommandManager>();

			//throw new NotImplementedException();
			DelegateCommand closeCommand = new DelegateCommand(CloseCommand);
			DelegateCommand openCommand = new DelegateCommand(OpenDocument);

			manager.RegisterCommand("EXIT", closeCommand);
			manager.RegisterCommand("OPEN", openCommand);
		}

		private void CloseCommand()
		{
			App.Current.Shutdown();
		}

		private void OpenDocument()
		{
			IOpenFileService service = _container.Resolve<IOpenFileService>();
			service.Open();
		}
	}
}

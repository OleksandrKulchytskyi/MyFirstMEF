using FirstPrismApp.Infrastructure;
using FirstPrismApp.Infrastructure.Menu;
using FirstPrismApp.Infrastructure.Services;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;

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
			LoadMenus();
			//LoadToolbar();
		}

		private void InitializeCoreServices()
		{
			_container.RegisterType<ICommandManager,FirstPrismApp.Infrastructure.CommandManager>(new Microsoft.Practices.Unity.ContainerControlledLifetimeManager());
			_container.RegisterType<AbstractMenuItem, MenuItemViewModel>(new ContainerControlledLifetimeManager(), 
																		new InjectionConstructor(new InjectionParameter(typeof(string), "$MAIN$"), 
																		new InjectionParameter(typeof(int), 1), 
																		new InjectionParameter(typeof(ImageSource), null), 
																		new InjectionParameter(typeof(ICommand), null), 
																		new InjectionParameter(typeof(KeyGesture), null), 
																		new InjectionParameter(typeof(bool), false), 
																		new InjectionParameter(typeof(IUnityContainer), this._container)));
		}

		private void LoadCommands()
		{
			ICommandManager manager = _container.Resolve<ICommandManager>();

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

		private void LoadMenus()
		{
			ICommandManager manager = _container.Resolve<ICommandManager>();
			AbstractMenuItem vm = _container.Resolve<AbstractMenuItem>();

			vm.Add(new MenuItemViewModel("_File", 1));
			vm.Get("_File").Add(new MenuItemViewModel("Exit", 8, null, manager.GetCommand("EXIT"), new KeyGesture(Key.F4, ModifierKeys.Alt, "Alt + F4")));
		}
	}
}

using FirstPrismApp.Infrastructure;
using FirstPrismApp.Infrastructure.Menu;
using FirstPrismApp.Infrastructure.Services;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using System;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MainApp
{
	internal class Loader
	{
		private IUnityContainer _container;

		public Loader(IUnityContainer container)
		{
			_container = container;
			InitializeCoreServices();
			LoadCommands();
			LoadMenus();

			//LoadTheme();
			//LoadToolbar();
		}

		private void InitializeCoreServices()
		{
			_container.RegisterType<ICommandManager, FirstPrismApp.Infrastructure.CommandManager>(new ContainerControlledLifetimeManager());
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
			ICommandManager cmdManager = _container.Resolve<ICommandManager>();
			AbstractMenuItem vm = _container.Resolve<AbstractMenuItem>();

			vm.Add(new MenuItemViewModel("_File", 1));

			vm.Get("_File").Add((new MenuItemViewModel("_Open", 2, new BitmapImage(new Uri(@"pack://application:,,,/MainApp;component/Icons/OpenFD.png")),
														cmdManager.GetCommand("OPEN"), new KeyGesture(Key.O, ModifierKeys.Control, "Ctrl + O"))));
			vm.Get("_File").Add(new MenuItemViewModel("E_xit", 3, null, cmdManager.GetCommand("EXIT"), new KeyGesture(Key.F4, ModifierKeys.Alt, "Alt + F4"), false, _container));

		}
	}
}
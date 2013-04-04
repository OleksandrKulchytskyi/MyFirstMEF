using Core.Infrastructure;
using Core.Infrastructure.Base;
using Core.Infrastructure.Events;
using Core.Infrastructure.Menu;
using Core.Infrastructure.Services;
using Core.Infrastructure.Themes;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using System;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MainApp
{
	internal class Loader
	{
		private int moduleLoaded = -1;
		private IUnityContainer _container = null;

		//private ILogger _logger = null;

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
			_container.RegisterType<IFileHistoryService, FileHistoryService>(new ContainerControlledLifetimeManager());

			_container.RegisterType<IStateService, ApplicationStateService>(new ContainerControlledLifetimeManager());
			_container.RegisterType<IThemeManager, ThemeManager>(new ContainerControlledLifetimeManager());
			_container.RegisterType<ICommandManager, Core.Infrastructure.CommandManager>(new ContainerControlledLifetimeManager());
			_container.RegisterType<AbstractMenuItem, MenuItemViewModel>(new ContainerControlledLifetimeManager(),
																		new InjectionConstructor(new InjectionParameter(typeof(string), "$MAIN$"),
																		new InjectionParameter(typeof(int), 1),
																		new InjectionParameter(typeof(ImageSource), null),
																		new InjectionParameter(typeof(ICommand), null),
																		new InjectionParameter(typeof(KeyGesture), null),
																		new InjectionParameter(typeof(bool), false),
																		new InjectionParameter(typeof(IUnityContainer), this._container)));

			_container.RegisterType<IWindowMapper, WindowMapper>(new ContainerControlledLifetimeManager());
			_container.RegisterType<IWindowProvider, WindowProviderService>(new ContainerControlledLifetimeManager());
		}

		private void LoadCommands()
		{
			ICommandManager manager = _container.Resolve<ICommandManager>();

			DelegateCommand closeCommand = new DelegateCommand(CloseCommand, CanClose);
			DelegateCommand exitCommand = new DelegateCommand(ExitCommand);
			DelegateCommand openCommand = new DelegateCommand(OpenDocument);
			DelegateCommand viewFullSourceCommand = new DelegateCommand(ViewFullSource, CanViewFullSource);
			DelegateCommand<object> openRecentFileCommand = new DelegateCommand<object>(OpenRecentDocument);

			manager.RegisterCommand("OPEN", openCommand);
			manager.RegisterCommand("CLOSE", closeCommand);
			manager.RegisterCommand("EXIT", exitCommand);
			manager.RegisterCommand("VIEWFULL", viewFullSourceCommand);
			manager.RegisterCommand("OPENRECENT", openRecentFileCommand);
			manager.Refresh();
		}

		#region command internals

		private void LoadTheme()
		{
			IThemeManager manager = _container.Resolve<IThemeManager>();

			//manager.AddTheme(new VS2010());
			//manager.SetCurrent("VS2010");
			//manager.AddTheme(new LightTheme());
			//manager.SetCurrent("Light");
			manager.AddTheme(new DarkTheme());
			manager.SetCurrent("Dark");
		}

		private void ExitCommand()
		{
			App.Current.Shutdown();
		}

		private void OpenDocument()
		{
			IOpenFileService service = _container.Resolve<IOpenFileService>();
			service.Open();
		}

		private void CloseCommand()
		{
			var evnt = new CloseDocumentEvent() { PathToDocument = _container.Resolve<IStateService>().GetCurrentDocument() };
			_container.Resolve<IEventAggregator>().GetEvent<CloseDocumentEvent>().Publish(evnt);
			_container.Resolve<IStateService>().CloseDocument();
		}

		private bool CanClose()
		{
			return _container.Resolve<IStateService>().GetCurrentDocument() != null;
		}

		private void ViewFullSource()
		{
			if (moduleLoaded < 0)
			{
				var manager = _container.Resolve<Microsoft.Practices.Prism.Modularity.IModuleManager>();
				if (manager != null)
				{
					manager.LoadModule("FullViewModule");
					moduleLoaded = 1;
				}
			}
			else
			{
				var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName.IndexOf("FullViewModule", StringComparison.OrdinalIgnoreCase) != -1);
				if (assembly != null)
				{
					Type t = assembly.GetTypes().FirstOrDefault(x => x.Name.Equals("IFullViewViewModel", StringComparison.OrdinalIgnoreCase));
					if (t != null)
					{
						IViewModel vm = _container.Resolve(t) as IViewModel;
						(vm.View as IWindow).Owner = System.Windows.Application.Current.MainWindow;
						(vm.View as IWindow).WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
						(vm.View as IWindow).Title = "Full view";
						(vm.View as IWindow).WindowStyle = System.Windows.WindowStyle.ToolWindow;
						(vm.View as IWindow).Show();
						(vm.View as IWindow).Show();
						vm = null;
					}
				}
			}
		}

		private bool CanViewFullSource()
		{
			return _container.Resolve<IStateService>().GetCurrentDocument() != null;
		}

		private void OpenRecentDocument(object file)
		{
			if (file == null) return;
			IOpenFileService service = _container.Resolve<IOpenFileService>();
			service.OpenFromID(file as string);
		}

		#endregion command internals

		private void LoadMenus()
		{
			ICommandManager cmdManager = _container.Resolve<ICommandManager>();
			AbstractMenuItem vm = _container.Resolve<AbstractMenuItem>();

			vm.Add(new MenuItemViewModel("_File", 1));

			vm.Get("_File").Add((new MenuItemViewModel("_Open", 1, new BitmapImage(new Uri(@"pack://application:,,,/MainApp;component/Icons/OpenFD.png")),
														cmdManager.GetCommand("OPEN"), new KeyGesture(Key.O, ModifierKeys.Control, "Ctrl + O"))));
			vm.Get("_File").Add((new MenuItemViewModel("_Close", 2, null, cmdManager.GetCommand("CLOSE"),
																		new KeyGesture(Key.F4, ModifierKeys.Control, "Ctrl + F4"))));
			vm.Get("_File").Add(new RecentMenuItemViewModel("Recent", 3, _container));
			vm.Get("_File").Add(MenuItemViewModel.Separator(3));
			vm.Get("_File").Add(new MenuItemViewModel("_Exit", 4, null, cmdManager.GetCommand("EXIT"), new KeyGesture(Key.F4, ModifierKeys.Alt, "Alt + F4"), false, _container));

			//added for proper recent file loading and displaying features.
			_container.Resolve<IFileHistoryService>().InitializeFromFile();

			vm.Add(new MenuItemViewModel("_View", 2));
			vm.Get("_View").Add((new MenuItemViewModel("_Full view", 1, null, cmdManager.GetCommand("VIEWFULL"),
																		new KeyGesture(Key.L, ModifierKeys.Control, "Ctrl + L"))));
		}
	}
}
using FirstPrismApp.Infrastructure.Base;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using System;

namespace FullViewModule
{
	[Module(ModuleName = "FullViewModule", OnDemand = true)]
	public sealed class FullViewModule : IModule
	{
		private const string _windowScope = "FullViewWindow";
		private GenericWeakReference<IUnityContainer> _weak;

		public FullViewModule(IUnityContainer container)
		{
			_weak = new GenericWeakReference<IUnityContainer>(container);
		}

		public void Initialize()
		{
			if (_weak.IsAlive && _weak.Get() != null)
			{
				_weak.Get().RegisterType<IFullView, FullViewWindow>();
				_weak.Get().RegisterType<IFullViewViewModel, FullViewViewModel>();

				var mapper = _weak.Get().Resolve<IWindowMapper>();
				if (mapper.Get(_windowScope) == null)
					mapper.Map(_windowScope, typeof(FullViewWindow));

				IWindow wind = _weak.Get().Resolve<IWindowProvider>().Generate(_windowScope);
				if (wind != null)
				{
					wind.Owner = System.Windows.Application.Current.MainWindow;
					wind.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
					wind.Title = "Full view";
					wind.WindowStyle = System.Windows.WindowStyle.ToolWindow;
					wind.Show();
				}
			}
		}
	}
}
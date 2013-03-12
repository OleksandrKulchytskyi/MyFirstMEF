using FirstPrismApp.Infrastructure;
using FirstPrismApp.Infrastructure.Base;
using FirstPrismApp.Infrastructure.Menu;
using Microsoft.Practices.Unity;
using System.Collections.Generic;

namespace ToolbarModule
{
	public class ToolbarViewViewModel : ViewModelBase, IToolbarViewViewModel
	{
		private IUnityContainer _container;

		public ToolbarViewViewModel(IToolbarView view, IUnityContainer container)
			: base(view)
		{
			_container = container;
			MainMenu = _container.Resolve<AbstractMenuItem>() as MenuItemViewModel;
		}

		private MenuItemViewModel _menu = null;

		public MenuItemViewModel MainMenu
		{
			get { return _menu; }
			set { _menu = value; RaisePropertyChanged("Menus"); }
		}

		public IList<AbstractCommandable> Menus
		{
			get { return _menu == null ? null : _menu.Children; }
		}
	}
}
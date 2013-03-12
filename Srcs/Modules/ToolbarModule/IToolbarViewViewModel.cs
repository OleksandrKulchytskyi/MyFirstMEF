using FirstPrismApp.Infrastructure;
using FirstPrismApp.Infrastructure.Base;
using FirstPrismApp.Infrastructure.Menu;
using System.Collections.Generic;

namespace ToolbarModule
{
	public interface IToolbarViewViewModel : IViewModel
	{
		MenuItemViewModel MainMenu { get; set; }

		IList<AbstractCommandable> Menus { get; }
	}
}
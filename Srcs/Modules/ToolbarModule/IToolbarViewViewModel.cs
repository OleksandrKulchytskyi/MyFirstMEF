using Core.Infrastructure;
using Core.Infrastructure.Base;
using Core.Infrastructure.Menu;
using System.Collections.Generic;

namespace ToolbarModule
{
	public interface IToolbarViewViewModel : IViewModel
	{
		MenuItemViewModel MainMenu { get; set; }

		IList<AbstractCommandable> Menus { get; }
	}
}
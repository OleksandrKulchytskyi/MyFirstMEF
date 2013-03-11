using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;

namespace FirstPrismApp.Infrastructure.Menu
{
	public sealed class MenuItemViewModel : AbstractMenuItem
	{
		#region CTOR
		public MenuItemViewModel(string header, int priority, ImageSource icon = null, 
								ICommand command = null, KeyGesture gesture = null, bool isCheckable = false, 
								IUnityContainer container = null)
			: base(header, priority, icon, command, gesture, isCheckable)
		{

		}
		#endregion

		#region Static
		public static AbstractMenuItem Separator(int priority)
		{
			return new MenuItemViewModel("SEP", priority);
		}
		#endregion
	}
}

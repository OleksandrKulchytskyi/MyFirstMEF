using FirstPrismApp.Infrastructure.Base;
using FirstPrismApp.Infrastructure.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;

namespace FirstPrismApp.Infrastructure.Menu
{
	public class MenuItemViewModel : AbstractMenuItem
	{
		#region ctor
		public MenuItemViewModel(string header, int priority, ImageSource icon = null, ICommand command = null,
								KeyGesture gesture = null, bool isCheckable = false, IUnityContainer container = null)
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

	public sealed class RecentMenuItemViewModel : MenuItemViewModel
	{
		GenericWeakReference<IUnityContainer> _containerWeak;
		GenericWeakReference<IFileHistoryService> _historyServWeak;

		public RecentMenuItemViewModel(string header, int priority, IUnityContainer container = null, ImageSource icon = null,
								ICommand command = null, KeyGesture gesture = null, bool isCheckable = false)
			: base(header, priority, icon, command, gesture, isCheckable)
		{
			if (container != null)
			{
				_containerWeak = new GenericWeakReference<IUnityContainer>(container);
				IFileHistoryService service = _containerWeak.Get().Resolve<IFileHistoryService>();
				if (service != null)
				{
					_historyServWeak = new GenericWeakReference<IFileHistoryService>(service);
					_historyServWeak.Get().RecentChanged += RecentMenuItemViewModel_RecentChanged;
				}
			}
		}

		private void RecentMenuItemViewModel_RecentChanged(object sender, RecentEventArgs e)
		{
			ICommandManager commands = _containerWeak.Get().Resolve<ICommandManager>();
			switch (e.Action)
			{
				case RecentAction.Added:
					ICommand openRecentCmd = commands.GetCommand("OPENRECENT");
					var newVM = new MenuItemViewModel(e.Item, Children.Count + 1, null, openRecentCmd);
					newVM.CommandParameter = newVM.Header;
					Add(newVM);
					break;
				case RecentAction.Removed:
					Remove(e.Item);
					break;

				case RecentAction.None:
				default:
					break;
			}
			commands = null;
		}
	}
}

using FirstPrismApp.Infrastructure.Base;
using FirstPrismApp.Infrastructure.Events;
using Microsoft.Practices.Prism.Events;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace FirstPrismApp.Infrastructure.Services
{
	public sealed class ThemeManager : IThemeManager
	{
		private static readonly Dictionary<string, ITheme> _themeDictionary = new Dictionary<string, ITheme>();
		private IEventAggregator _eventAggregator;
		//private ILoggerService _logger;

		public ThemeManager(IEventAggregator eventAggregator)//, ILoggerService logger)
		{
			Themes = new ObservableCollection<ITheme>();
			_eventAggregator = eventAggregator;
			//_logger = logger;
		}

		public ObservableCollection<ITheme> Themes { get; internal set; }

		public ITheme CurrentTheme { get; internal set; }

		public bool SetCurrent(string name)
		{
			if (_themeDictionary.ContainsKey(name))
			{
				ITheme newTheme = _themeDictionary[name];
				this.CurrentTheme = newTheme;

				ResourceDictionary theme = Application.Current.MainWindow.Resources.MergedDictionaries[0];
				ResourceDictionary appTheme = Application.Current.Resources.MergedDictionaries.Count > 0 ? Application.Current.Resources.MergedDictionaries[0] : null;
				theme.MergedDictionaries.Clear();
				if (appTheme != null)
					appTheme.MergedDictionaries.Clear();
				else
				{
					appTheme = new ResourceDictionary();
					Application.Current.Resources.MergedDictionaries.Add(appTheme);
				}
				appTheme.MergedDictionaries.Clear();
				foreach (var uri in newTheme.UriList)
				{
					theme.MergedDictionaries.Add(new ResourceDictionary() { Source = uri });
					if (uri.ToString().Contains("AvalonDock") && appTheme != null)
					{
						appTheme.MergedDictionaries.Add(new ResourceDictionary() { Source = uri });
					}
				}

				//_logger.Log("Theme set to " + name, LogCategory.Info, LogPriority.None);
				_eventAggregator.GetEvent<ThemeChangeEvent>().Publish(newTheme);
			}
			return false;
		}

		public bool AddTheme(ITheme theme)
		{
			if (!_themeDictionary.ContainsKey(theme.Name))
			{
				_themeDictionary.Add(theme.Name, theme);
				Themes.Add(theme);
				return true;
			}
			return false;
		}
	}
}
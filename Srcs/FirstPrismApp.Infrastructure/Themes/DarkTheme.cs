using Core.Infrastructure.Base;
using System;
using System.Collections.Generic;

namespace Core.Infrastructure.Themes
{
	public sealed class DarkTheme : ITheme
	{
		public DarkTheme()
		{
			this.UriList = new List<Uri>();
			this.UriList.Add(new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Colours.xaml"));
			this.UriList.Add(new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml"));
			this.UriList.Add(new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml"));
			this.UriList.Add(new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/Blue.xaml"));
			this.UriList.Add(new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/BaseLight.xaml"));
			this.UriList.Add(new Uri("pack://application:,,,/Core.Infrastructure;component/Styles/DarkTheme.xaml"));
		}

		public IList<Uri> UriList { get; internal set; }

		public string Name
		{
			get { return "Dark"; }
		}
	}
}
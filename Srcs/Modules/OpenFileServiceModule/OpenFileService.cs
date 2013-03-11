using FirstPrismApp.Infrastructure.Events;
using FirstPrismApp.Infrastructure.Services;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenFileServiceModule
{
	public sealed class OpenFileService : IOpenFileService
	{
		private IUnityContainer _container;
		private IEventAggregator _eventAggregator;

		public OpenFileService(IUnityContainer container, IEventAggregator eventAggregator)
		{
			_container = container;
			_eventAggregator = eventAggregator;
		}

		public string Open(object location = null)
		{
			OpenFileDialog dialog = new OpenFileDialog();
			bool? result = false;

			if (location == null)
			{
				result = dialog.ShowDialog();
				location = dialog.FileName;
			}
			else
			{
				result = true;
			}

			if (result == true && !string.IsNullOrWhiteSpace(location.ToString()))
			{
				DocumentLoadingEvent openValue = new DocumentLoadingEvent() { Path = location.ToString() };
				_eventAggregator.GetEvent<DocumentLoadingEvent>().Publish(openValue);
				return location.ToString();
			}
			return string.Empty;
		}

		public string OpenFromID(string contentID)
		{
			throw new NotImplementedException();
		}
	}
}

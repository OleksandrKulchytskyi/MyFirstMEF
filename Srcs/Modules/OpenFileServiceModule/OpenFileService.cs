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
				result = true;

			if (result == true && !string.IsNullOrWhiteSpace(location.ToString()))
			{
				string loc = location.ToString();
				_container.Resolve<IStateService>().AddToRecentAndSetCurrent(loc);
				IFileHistoryService serv = _container.Resolve<IFileHistoryService>();
				if (serv.AddToRecent(loc))
					serv.UpdateStorage().ContinueWith(prevTask =>
					{
						var exc = prevTask.Exception.Flatten().InnerException;
						_container.Resolve<FirstPrismApp.Infrastructure.Base.ILogger>()
							.Log(FirstPrismApp.Infrastructure.Base.LogSeverity.Error, exc.Message, exc);
						prevTask.Dispose();
					}, System.Threading.Tasks.TaskContinuationOptions.NotOnRanToCompletion);

				DocumentLoadingEvent openValue = new DocumentLoadingEvent() { Path = loc };
				_eventAggregator.GetEvent<DocumentLoadingEvent>().Publish(openValue);
				return loc;
			}
			return string.Empty;
		}

		public string OpenFromID(string contentID)
		{
			if (!string.IsNullOrWhiteSpace(contentID))
			{
				_container.Resolve<IStateService>().AddToRecentAndSetCurrent(contentID);
				IFileHistoryService serv = _container.Resolve<IFileHistoryService>();
				if (serv.AddToRecent(contentID))
					serv.UpdateStorage().ContinueWith(prevTask =>
					{
						var exc = prevTask.Exception.Flatten().InnerException;
						_container.Resolve<FirstPrismApp.Infrastructure.Base.ILogger>()
							.Log(FirstPrismApp.Infrastructure.Base.LogSeverity.Error, exc.Message, exc);
						prevTask.Dispose();
					}, System.Threading.Tasks.TaskContinuationOptions.NotOnRanToCompletion);

				DocumentLoadingEvent openValue = new DocumentLoadingEvent() { Path = contentID };
				_eventAggregator.GetEvent<DocumentLoadingEvent>().Publish(openValue);
				return contentID;
			}
			return string.Empty;
		}
	}
}

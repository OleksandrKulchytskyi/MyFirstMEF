using Business.Common;
using FirstPrismApp.Infrastructure;
using FirstPrismApp.Infrastructure.Events;
using FirstPrismApp.Infrastructure.Services;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using System;

namespace DetailedViewModule
{
	public sealed class DetailedViewViewModel : ViewModelBase, IDetailedViewViewModel, IDisposable
	{
		private int disposed = 0;

		private IUnityContainer _container = null;
		private IEventAggregator _evntAgg = null;
		private SubscriptionToken _closeSubsToken = null;
		private SubscriptionToken _entryChangedToken = null;

		public DetailedViewViewModel(IDetailedView view, IEventAggregator eventAgg, IUnityContainer container)
			: base(view)
		{
			_container = container;
			_evntAgg = eventAgg;
			_entryChangedToken = _evntAgg.GetEvent<EntryChangedEvent>().Subscribe(OnEntryChanged);
			_closeSubsToken = _evntAgg.GetEvent<CloseDocumentEvent>().Subscribe(OnClosingDocument);
		}

		#region Property Entry
		private LogEntryDescription _Entry;
		public LogEntryDescription Entry
		{
			get
			{
				return _Entry;
			}
			set
			{
				if (_Entry != value)
				{
					_Entry = value;
					RaisePropertyChanged("Entry");
				}
			}
		}
		#endregion

		private void OnClosingDocument(CloseDocumentEvent args)
		{
			if (Entry != null)
				Entry.Content = string.Empty;
		}

		private void OnEntryChanged(EntryChangedEvent args)
		{
			if (args.SelectedItem == null) return;

			if (Entry == null)
				Entry = new LogEntryDescription();
			string currentDoc = _container.Resolve<IStateService>().GetCurrentDocument();
			Entry.Content = _container.Resolve<IEntryContentService>().GetErrorContentForLine(currentDoc, args.SelectedItem.LineNumber);
			Entry.Severity = args.SelectedItem.Severity;
			Entry.Time = args.SelectedItem.Time;
		}

		public void Dispose()
		{
			if (disposed == 1) return;
			disposed = 1;

			if (_closeSubsToken != null)
			{
				_evntAgg.GetEvent<CloseDocumentEvent>().Unsubscribe(_closeSubsToken); _closeSubsToken = null;
			}
			if (_entryChangedToken != null)
			{
				_evntAgg.GetEvent<EntryChangedEvent>().Unsubscribe(_entryChangedToken); _entryChangedToken = null;
			}

			if (_evntAgg != null) _evntAgg = null;
		}
	}
}
using Business.Common;
using FirstPrismApp.Infrastructure;
using FirstPrismApp.Infrastructure.Base;
using FirstPrismApp.Infrastructure.Events;
using FirstPrismApp.Infrastructure.Services;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace ItemsViewModule
{
	public sealed class ItemsViewViewModel : ViewModelBase, IItemsViewViewModel, IDisposable
	{
		private int disposed = 0;
		private IUnityContainer _container = null;
		private IEventAggregator _eventMgr = null;
		private SubscriptionToken _subToken = null;
		private SubscriptionToken _subTokenClosing = null;

		public ItemsViewViewModel(IItemsView view, IEventAggregator eventAgg, IUnityContainer cont)
			: base(view)
		{
			_container = cont;
			_eventMgr = eventAgg;
			_subToken = _eventMgr.GetEvent<DocumentLoadingEvent>().Subscribe(OnLoadingDocument);
			_subTokenClosing = _eventMgr.GetEvent<CloseDocumentEvent>().Subscribe(OnClosingDocument);
		}

		private LogItem _SelectedEntry;
		public LogItem SelectedEntry
		{
			get
			{
				return _SelectedEntry;
			}
			set
			{
				if (_SelectedEntry != value)
				{
					_SelectedEntry = value;
					RaisePropertyChanged("SelectedEntry");
					_eventMgr.GetEvent<EntryChangedEvent>().Publish(new EntryChangedEvent() { SelectedItem = _SelectedEntry });
				}
			}
		}

		private ObservableCollection<LogItem> _Entries;
		public ObservableCollection<LogItem> Entries
		{
			get
			{
				return _Entries;
			}
			set
			{
				if (_Entries != value)
				{
					_Entries = value;
					RaisePropertyChanged("Entries");
				}
			}
		}

		private bool _IsBusy;
		public bool IsBusy
		{
			get
			{
				return _IsBusy;
			}
			set
			{
				if (_IsBusy != value)
				{
					_IsBusy = value;
					RaisePropertyChanged("IsBusy");
				}
			}
		}

		private void OnLoadingDocument(DocumentLoadingEvent args)
		{
			IsBusy = true;
			try
			{
				_container.Resolve<ILogger>().Log(LogSeverity.Info, string.Format("Begin load document: {0}", args.Path), null);
				System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
				IParsingService service = _container.Resolve<IParsingService>();
				Entries = new ObservableCollection<LogItem>(service.ParseLog(args.Path));
				_container.Resolve<IStateService>().AddToRecentAndSetCurrent(args.Path);
				_container.Resolve<ICommandManager>().Refresh();
			}
			catch (Exception ex)
			{
				_container.Resolve<ILogger>().Log(LogSeverity.Error, ex.Message, ex);
				MessageBox.Show(System.Windows.Application.Current.MainWindow, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			finally
			{
				IsBusy = false;
			}
		}

		private void OnClosingDocument(CloseDocumentEvent args)
		{
			_container.Resolve<ILogger>().Log(LogSeverity.Info, string.Format("Begin document close operation: {0}", args.PathToDocument), null);
			Entries.Clear();
			_container.Resolve<ICommandManager>().Refresh();
		}

		public void Dispose()
		{
			if (disposed == 1) return;
			disposed = 1;

			if (_subToken != null)
			{
				_eventMgr.GetEvent<DocumentLoadingEvent>().Unsubscribe(_subToken); _subToken = null;
				_eventMgr.GetEvent<CloseDocumentEvent>().Unsubscribe(_subTokenClosing); _subTokenClosing = null;
			}

			if (_eventMgr != null) _eventMgr = null;
			if (_container != null) _container = null;
		}
	}
}
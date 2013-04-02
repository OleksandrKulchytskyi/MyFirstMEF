using FirstPrismApp.Infrastructure;
using FirstPrismApp.Infrastructure.Base;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using System;

namespace FullViewModule
{
	public sealed class FullViewViewModel : ViewModelBase, IFullViewViewModel, IDisposable
	{
		private int disposed = 0;
		private GenericWeakReference<IUnityContainer> _container = null;
		private GenericWeakReference<IEventAggregator> _eventMgr = null;

		public FullViewViewModel(IView view, IEventAggregator eventAgg, IUnityContainer cont)
			: base(view)
		{
			_container = new GenericWeakReference<IUnityContainer>(cont);
			_eventMgr = new GenericWeakReference<IEventAggregator>(eventAgg);
		}

		public void Dispose()
		{
			if (disposed == 1) return;
			disposed = 1;

			if (_eventMgr != null) _eventMgr = null;
			if (_container != null) _container = null;
		}

		#region Property DocumentPath
		private string _DocumentPath;
		public string DocumentPath
		{
			get
			{
				return _DocumentPath;
			}
			set
			{
				if (_DocumentPath != value)
				{
					_DocumentPath = value;
					RaisePropertyChanged("DocumnetPath");
				}
			}
		}
		#endregion
	}
}
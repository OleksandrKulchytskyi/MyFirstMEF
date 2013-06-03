using Business.Common;
using Core.Infrastructure;
using Core.Infrastructure.Helpers;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;

namespace ItemsViewModule
{
	public sealed class ItemsViewModule : IModule, IDisposable
	{
		private int disposed = 0;
		private IUnityContainer _container;
		private IRegionManager _manager;

		public ItemsViewModule(IUnityContainer cont, IRegionManager manager)
		{
			_container = cont;
			_manager = manager;
		}

		public void Initialize()
		{
			_container.RegisterType<IItemsView, ItemsView>();
			_container.RegisterType<IItemsViewViewModel, ItemsViewViewModel>(new ContainerControlledLifetimeManager());

			IRegion region = _manager.Regions[RegionConstants.ItemsRegion];

			var vm = _container.Resolve<IItemsViewViewModel>();
			//vm.Entries = new System.Collections.ObjectModel.ObservableCollection<Business.Common.LogItem>();
			region.Add(vm.View);
		}

		public void Dispose()
		{
			if (disposed == 1) return;

			disposed = 1;
			_container = null;
			_manager = null;
		}
	}
}
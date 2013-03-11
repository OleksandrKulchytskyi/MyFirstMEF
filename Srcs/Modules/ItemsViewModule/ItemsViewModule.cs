using FirstPrismApp.Infrastructure;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace ItemsViewModule
{
	public class ItemsViewModule : IModule
	{
		IUnityContainer _container;
		IRegionManager _manager;

		public ItemsViewModule(IUnityContainer cont, IRegionManager manager)
		{
			_container = cont;
			_manager = manager;
		}

		public void Initialize()
		{
			_container.RegisterType<IItemsView, ItemsView>();
			_container.RegisterType<IItemsViewViewModel, ItemsViewViewModel>();

			IRegion region = _manager.Regions[RegionConstants.ItemsRegion];

			var vm = _container.Resolve<IItemsViewViewModel>();
			vm.Entries = new System.Collections.ObjectModel.ObservableCollection<Business.Common.LogItem>();
			region.Add(vm.View);
		}
	}
}
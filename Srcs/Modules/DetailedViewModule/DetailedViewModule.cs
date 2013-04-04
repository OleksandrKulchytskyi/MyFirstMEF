using Core.Infrastructure;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace DetailedViewModule
{
	public class DetailedViewModule : IModule
	{
		private IUnityContainer _container;
		private IRegionManager _regionManager;

		public DetailedViewModule(IUnityContainer container, IRegionManager regionManager)
		{
			_regionManager = regionManager;
			_container = container;
		}

		public void Initialize()
		{
			_container.RegisterType<IDetailedView, DetailedView>();
			_container.RegisterType<IDetailedViewViewModel, DetailedViewViewModel>(new ContainerControlledLifetimeManager());

			var vm = _container.Resolve<IDetailedViewViewModel>();
			vm.Entry = new Business.Common.LogEntryDescription();
			vm.Entry.Content = "No data";
			_regionManager.Regions[RegionConstants.DetailsRegion].Add(vm.View);
		}
	}
}
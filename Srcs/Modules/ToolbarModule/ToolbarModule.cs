using FirstPrismApp.Infrastructure;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;

namespace ToolbarModule
{
	public class ToolbarModule : IModule
	{
		private IUnityContainer _container;
		private IRegionManager _regionManager;

		public ToolbarModule(IUnityContainer container, IRegionManager regionManager)
		{
			_regionManager = regionManager;
			_container = container;
		}

		public void Initialize()
		{
			_container.RegisterType<IToolbarView, ToolbarView>();
			_container.RegisterType<IToolbarViewViewModel, ToolbarViewViewModel>();

			IRegion region = _regionManager.Regions[RegionConstants.ToolbarRegion];
			var vm = _container.Resolve<IToolbarViewViewModel>();
			region.Add(vm.View);
		}
	}
}
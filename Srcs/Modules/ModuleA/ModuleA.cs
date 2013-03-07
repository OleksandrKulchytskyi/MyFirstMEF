using FirstPrismApp.Infrastructure;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace ModuleA
{
	[Module(ModuleName = "ModuleA", OnDemand = true)]
	public class ModuleA : IModule
	{
		private IUnityContainer _container;
		private IRegionManager _regionManager;

		public ModuleA(IUnityContainer container, IRegionManager regionManager)
		{
			_regionManager = regionManager;
			_container = container;
		}

		public void Initialize()
		{
			_container.RegisterType<IContentView, ContentView>();
			_container.RegisterType<IContentViewViewModel, ContentViewViewModel>();

			IRegion region = _regionManager.Regions[RegionConstants.ToolbarRegion];
			region.Add(_container.Resolve<ToolbarView>());
			region.Add(_container.Resolve<ToolbarView>());
			region.Add(_container.Resolve<ToolbarView>());
			region.Add(_container.Resolve<ToolbarView>());

			//_regionManager.RegisterViewWithRegion(RegionConstants.ToolbarRegion, typeof(ToolbarView));
			//_regionManager.RegisterViewWithRegion(RegionConstants.ContentRegion, typeof(ContentView));

			var vm = _container.Resolve<IContentViewViewModel>();
			vm.Message = "First View A";
			_regionManager.Regions[RegionConstants.ContentRegion].Add(vm.View);

			var vm2 = _container.Resolve<IContentViewViewModel>();
			vm2.Message = "Second View A";
			_regionManager.Regions[RegionConstants.ContentRegion].Add(vm2.View);
		}
	}
}
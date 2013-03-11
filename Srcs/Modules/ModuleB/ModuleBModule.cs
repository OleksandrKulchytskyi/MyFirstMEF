using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System.Windows;

namespace ModuleB
{
	[Module(ModuleName = "ModuleB", OnDemand = true)]
	//[ModuleDependency("")] // add some dependency to another modules.
	public class ModuleBModule : IModule
	{
		IUnityContainer _container;
		IRegionManager _regionManager;

		public ModuleBModule(IUnityContainer cont, IRegionManager regionManager)
		{
			_container = cont;
			_regionManager = regionManager;
		}

		public void Initialize()
		{
			_container.RegisterType<ContentView>();
			_container.RegisterType<ToolbarView>();
			_container.RegisterType<IContentViewViewModel, ContentViewViewMode>();

			_regionManager.RegisterViewWithRegion(FirstPrismApp.Infrastructure.RegionConstants.ItemsRegion, typeof(ContentView));

			MessageBox.Show("ModuleB is loaded");
		}
	}
}
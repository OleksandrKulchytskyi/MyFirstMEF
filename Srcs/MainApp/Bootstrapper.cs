using FirstPrismApp.Infrastructure;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using System;
using System.Windows;
using System.Windows.Controls;

namespace MainApp
{
	public class Bootstrapper : UnityBootstrapper
	{
		protected override System.Windows.DependencyObject CreateShell()
		{
			return Container.Resolve<Shell>();
		}

		protected override void InitializeShell()
		{
			base.InitializeShell();
			App.Current.MainWindow = (Window)Shell;
			App.Current.MainWindow.Show();
		}

		//protected override void ConfigureModuleCatalog()
		//{
		//	base.ConfigureModuleCatalog();
		//	ModuleCatalog moduleCatalog = (ModuleCatalog)this.ModuleCatalog;
		//	moduleCatalog.AddModule(typeof(ModuleA.ModuleA));

		//	Type mduleBType = typeof(ModuleB.ModuleBModule);
		//	ModuleCatalog.AddModule(new ModuleInfo()
		//	{
		//		ModuleName = mduleBType.Name,
		//		ModuleType = mduleBType.AssemblyQualifiedName,
		//		InitializationMode = InitializationMode.WhenAvailable
		//	});
		//}

		protected override void InitializeModules()
		{
			base.InitializeModules();
			Loader loader = new Loader(Container);
		}

		protected override Microsoft.Practices.Prism.Regions.RegionAdapterMappings ConfigureRegionAdapterMappings()
		{
			RegionAdapterMappings mappings = base.ConfigureRegionAdapterMappings();
			mappings.RegisterMapping(typeof(StackPanel), Container.Resolve<StackPanelRegionAdapter>());
			return mappings;
		}

		protected override IModuleCatalog CreateModuleCatalog()
		{
			//return new DirectoryModuleCatalog() { ModulePath = @".\Modules" }; //to load modules from directory
			//return Microsoft.Practices.Prism.Modularity.ModuleCatalog.CreateFromXaml(new Uri("/MainApp;component/XamlCatalog.xaml", UriKind.Relative));//to load modules from xaml file
			return new ConfigurationModuleCatalog();
		}
	}
}
using FirstPrismApp.Infrastructure;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

		protected override void ConfigureModuleCatalog()
		{
			base.ConfigureModuleCatalog();
			ModuleCatalog moduleCatalog = (ModuleCatalog)this.ModuleCatalog;
			moduleCatalog.AddModule(typeof(ModuleA.ModuleA));

			Type mduleBType = typeof(ModuleB.ModuleBModule);
			ModuleCatalog.AddModule(new ModuleInfo()
			{

				ModuleName = mduleBType.Name,
				ModuleType = mduleBType.AssemblyQualifiedName,
				InitializationMode = InitializationMode.WhenAvailable
			});
		}

		protected override Microsoft.Practices.Prism.Regions.RegionAdapterMappings ConfigureRegionAdapterMappings()
		{
			RegionAdapterMappings mappings = base.ConfigureRegionAdapterMappings();
			mappings.RegisterMapping(typeof(StackPanel), Container.Resolve<StackPanelRegionAdapter>());
			return mappings;
		}

		protected override IModuleCatalog CreateModuleCatalog()
		{
			//var cat= base.CreateModuleCatalog();
			return new DirectoryModuleCatalog() { ModulePath = @".\Modules" };
		}
	}
}

using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows;

namespace MefModuleA
{
	[ModuleExport(typeof(MefModuleA), InitializationMode = InitializationMode.WhenAvailable)]
	public class MefModuleA : IModule
	{
		IRegionManager _region;

		[ImportingConstructor]
		public MefModuleA(IRegionManager manager)
		{
			_region = manager;
		}

		public void Initialize()
		{
			var toolbarViewInst = ServiceLocator.Current.GetInstance<ToolbarView>();
			var contViewInst = ServiceLocator.Current.GetInstance<ContentView>();
			_region.Regions[FirstPrismApp.Infrastructure.RegionConstants.ToolbarRegion].Add(toolbarViewInst);
			_region.Regions[FirstPrismApp.Infrastructure.RegionConstants.ItemsRegion].Add(contViewInst);
			MessageBox.Show("MefModuleA is loaded.");
		}
	}
}

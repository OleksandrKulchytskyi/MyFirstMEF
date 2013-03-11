﻿using FirstPrismApp.Infrastructure;
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
			IRegion region = _regionManager.Regions[RegionConstants.ToolbarRegion];

			region.Add(_container.Resolve<ToolbarView>());
			region.Add(_container.Resolve<ToolbarView>());
			region.Add(_container.Resolve<ToolbarView>());
			region.Add(_container.Resolve<ToolbarView>());
		}
	}
}
using FirstPrismApp.Infrastructure.Services;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenFileServiceModule
{
	public sealed class OpenFileServiceModule : IModule
	{
		private IUnityContainer _container;
		public OpenFileServiceModule(IUnityContainer container)
		{
			_container = container;
		}

		public void Initialize()
		{
			_container.RegisterType<IOpenFileService, OpenFileService>(new ContainerControlledLifetimeManager());
		}
	}
}

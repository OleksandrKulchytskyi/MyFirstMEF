using Core.Infrastructure.Base;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace LoggerModule
{
	public sealed class LoggerModuleModule : IModule
	{
		private IUnityContainer _container = null;

		public LoggerModuleModule(IUnityContainer container)
		{
			_container = container;
		}

		public void Initialize()
		{
			_container.RegisterType<ILogger, LoggerService>(new Microsoft.Practices.Unity.ContainerControlledLifetimeManager());
		}
	}
}
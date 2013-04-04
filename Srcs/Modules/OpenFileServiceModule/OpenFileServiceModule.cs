using Core.Infrastructure.Services;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

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
			_container.RegisterType<IOpenFileService, OpenFileService>();
		}
	}
}
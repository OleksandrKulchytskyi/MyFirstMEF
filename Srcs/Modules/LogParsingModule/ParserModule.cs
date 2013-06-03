using Business.Common;
using Core.Infrastructure.Base;
using Core.Infrastructure.Helpers;
using Core.Infrastructure.Services;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace LogParsingModule
{
	public sealed class ParserModule : IModule
	{
		private IUnityContainer _container;

		public ParserModule(IUnityContainer container)
		{
			_container = container;
		}

		public void Initialize()
		{
			_container.RegisterType<IParsingService, LogParser>();

			_container.RegisterType<LogItemsPool, LogItemsPool>(new ContainerControlledLifetimeManager(),
				new InjectionConstructor(new InjectionParameter(typeof(int), 300)));
			_container.RegisterType<IEntryContentService, EntryContentService>();
			_container.RegisterType<IItemsProvider<LogItem>, LogEntriesProvider>();
		}
	}
}
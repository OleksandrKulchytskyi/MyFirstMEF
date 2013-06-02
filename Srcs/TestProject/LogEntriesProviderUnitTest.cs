using Core.Infrastructure.Helpers;
using LogParsingModule;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject
{
	[TestClass]
	public class LogEntriesProviderUnitTest
	{
		IUnityContainer container;

		[TestInitialize]
		public void Init()
		{
			container = new UnityContainer();
			container.RegisterType<LogItemsPool, LogItemsPool>(new InjectionConstructor(new InjectionParameter(typeof(int), 100)));
		}

		[TestMethod]
		public void LogEntriesProviderTest()
		{
			string path = @"D:\MyProj\github\Sharedeployed\ShareDeployed\ShareDeployed.Proxy.WpfTest\bin\Debug\Log\Wpferror.log";
			LogEntriesProvider provider = new LogEntriesProvider(container);
			int count = provider.FetchCount();
			Assert.IsTrue(count > 500);
			var range50 = provider.FetchRange(0, 10);
			var range150 = provider.FetchRange(10, 100);
			Assert.IsNotNull(range150);
			Assert.IsNotNull(range50);
		}
	}
}
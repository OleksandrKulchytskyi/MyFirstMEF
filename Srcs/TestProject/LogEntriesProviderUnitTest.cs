using LogParsingModule;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject
{
	[TestClass]
	public class LogEntriesProviderUnitTest
	{
		[TestMethod]
		public void LogEntriesProviderTest()
		{
			string path = @"D:\MyProj\github\Sharedeployed\ShareDeployed\ShareDeployed.Proxy.WpfTest\bin\Debug\Log\Wpferror.log";
			LogEntriesProvider provider = new LogEntriesProvider(path);
			int count = provider.FetchCount();
			Assert.IsTrue(count > 500);
			var range50 = provider.FetchRange(0, 10);
			var range150 = provider.FetchRange(10, 100);
			Assert.IsNotNull(range150);
			Assert.IsNotNull(range50);
		}
	}
}
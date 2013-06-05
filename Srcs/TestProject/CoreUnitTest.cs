using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Infrastructure.Helpers;

namespace TestProject
{
	[TestClass]
	public class CoreUnitTest
	{
		[TestMethod]
		public void TestMethod1()
		{
			SplitableList<int> split = new SplitableList<int>(50000);
			Assert.IsTrue(split.BucketsCount == 5);
			for (int i = 0; i < 50000; i++)
			{
				split.Add(i);
			}

			var data = split.GetRange(9999, 10000);
			Assert.IsTrue(data.Count == 10000);
		}
	}
}

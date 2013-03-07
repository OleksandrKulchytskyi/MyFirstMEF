using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace MefModuleA
{
	[ModuleExport(typeof(MefModuleA), InitializationMode = InitializationMode.WhenAvailable)]
	public class MefModuleA : IModule
	{
		public void Initialize()
		{
			MessageBox.Show("MefModuleA is loaded.");
		}
	}
}

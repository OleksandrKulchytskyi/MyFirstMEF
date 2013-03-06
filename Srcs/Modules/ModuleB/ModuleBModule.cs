using Microsoft.Practices.Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleB
{
	[Module(ModuleName = "ModuleB", OnDemand = true)]
	public class ModuleBModule : IModule
	{
		public void Initialize()
		{
			//throw new NotImplementedException();
		}
	}
}

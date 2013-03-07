using Microsoft.Practices.Prism.Modularity;
using System.Windows;

namespace ModuleB
{
	[Module(ModuleName = "ModuleB", OnDemand = true)]
	//[ModuleDependency("")] // add some dependency to another modules.
	public class ModuleBModule : IModule
	{
		public void Initialize()
		{
			MessageBox.Show("ModuleB is loaded");
		}
	}
}
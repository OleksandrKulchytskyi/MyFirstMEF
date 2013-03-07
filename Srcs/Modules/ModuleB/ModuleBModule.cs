using Microsoft.Practices.Prism.Modularity;

namespace ModuleB
{
	[Module(ModuleName = "ModuleB", OnDemand = true)]
	//[ModuleDependency("")] // add some dependency to another modules.
	public class ModuleBModule : IModule
	{
		public void Initialize()
		{
			//throw new NotImplementedException();
		}
	}
}
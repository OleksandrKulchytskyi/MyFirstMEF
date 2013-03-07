using System.ComponentModel.Composition.Hosting;
using System.Windows;

namespace MainAppMEF
{
	internal class Bootstrapper : Microsoft.Practices.Prism.MefExtensions.MefBootstrapper
	{
		protected override System.Windows.DependencyObject CreateShell()
		{
			return Container.GetExportedValue<Shell>();
		}

		protected override void InitializeShell()
		{
			base.InitializeShell();
			App.Current.MainWindow = (Window)Shell;
			App.Current.MainWindow.Show();
		}

		protected override void ConfigureAggregateCatalog()
		{
			base.ConfigureAggregateCatalog();
			AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(Bootstrapper).Assembly));
			AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(MefModuleA.MefModuleA).Assembly));
		}
	}
}
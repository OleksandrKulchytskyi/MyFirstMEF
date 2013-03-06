using System.Windows;

namespace MainApp
{
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			Bootstrapper boot = new Bootstrapper();
			boot.Run();
		}
	}
}
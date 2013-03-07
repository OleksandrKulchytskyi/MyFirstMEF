using System.Windows;

namespace MainAppMEF
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
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
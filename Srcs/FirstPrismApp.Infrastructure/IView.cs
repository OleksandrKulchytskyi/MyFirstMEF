namespace FirstPrismApp.Infrastructure
{
	public interface IView
	{
		IViewModel ViewModel { get; set; }
	}
}
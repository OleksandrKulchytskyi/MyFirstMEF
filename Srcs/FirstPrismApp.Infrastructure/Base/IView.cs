namespace Core.Infrastructure
{
	public interface IView
	{
		IViewModel ViewModel { get; set; }
	}
}
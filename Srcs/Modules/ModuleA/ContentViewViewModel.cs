namespace ModuleA
{
	public class ContentViewViewModel : IContentViewViewModel
	{
		public FirstPrismApp.Infrastructure.IView View { get; set; }

		public ContentViewViewModel(IContentView view)
		{
			View = view;
			View.ViewModel = this;
		}

		public string Message { get; set; }
	}
}
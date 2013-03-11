using System.ComponentModel;

namespace FirstPrismApp.Infrastructure
{
	public class ViewModelBase : IViewModel, INotifyPropertyChanged
	{
		public IView View { get; set; }

		public ViewModelBase(IView view)
		{
			View = view;
			View.ViewModel = this;
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string prop)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(prop));
			}
		}
	}
}
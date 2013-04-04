using System.ComponentModel;

namespace Core.Infrastructure
{
	public class ViewModelBase : IViewModel, INotifyPropertyChanged
	{
		public IView View { get; set; }

		public ViewModelBase(IView view)
		{
			View = view;
			if (View != null)
				View.ViewModel = this;
		}
		
		[field:System.NonSerialized]
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string prop)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(prop));
			}
		}

		protected void RaisePropertiesChanged(params string[] propertyNames)
		{
			if (propertyNames == null)
			{
				throw new System.ArgumentNullException("propertyNames");
			}
			string[] strArray = propertyNames;
			for (int i = 0; i < strArray.Length; i = (int)(i + 1))
			{
				string propertyName = strArray[i];
				this.RaisePropertyChanged(propertyName);
			}
		}
	}
}
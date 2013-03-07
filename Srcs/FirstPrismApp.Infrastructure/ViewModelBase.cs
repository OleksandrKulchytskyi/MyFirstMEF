using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

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

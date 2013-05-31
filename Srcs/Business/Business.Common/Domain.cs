using System;
using System.ComponentModel;

namespace Business.Common
{
	public class EntityBase : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string prop)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(prop));
			}
		}
	}

	public class LogItem
	{
		public int LineNumber { get; set; }

		public DateTime Time { get; set; }

		public string Severity { get; set; }
	}

	public class LogEntryDescription : EntityBase
	{
		#region Property Severity
		private string _Severity;
		public string Severity
		{
			get
			{
				return _Severity;
			}
			set
			{
				if (_Severity != value)
				{
					_Severity = value;
					RaisePropertyChanged("Severity");
				}
			}
		}
		#endregion

		public DateTime Time { get; set; }

		#region Property Content
		private string _Content;
		public string Content
		{
			get
			{
				return _Content;
			}
			set
			{
				if (_Content != value)
				{
					_Content = value;
					RaisePropertyChanged("Content");
				}
			}
		}
		#endregion
	}
}
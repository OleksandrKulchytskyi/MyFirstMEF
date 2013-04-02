using FirstPrismApp.Infrastructure;
using FirstPrismApp.Infrastructure.Base;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using System.Windows.Input;

namespace FullViewModule
{
	public enum ToggleEditorOption
	{
		WordWrap = 0,
		ShowLineNumber = 1,
		ShowEndOfLine = 2,
		ShowSpaces = 3,
		ShowTabs = 4
	}

	public interface IToolbarViewModel
	{
		bool WordWrap { get; set; }
	}

	public sealed class ToolbarViewModel : ViewModelBase, IToolbarViewModel
	{
		private GenericWeakReference<IEventAggregator> _eventAggr;

		public ToolbarViewModel(IEventAggregator eventAggr)
			: base(null)
		{
			_eventAggr = new GenericWeakReference<IEventAggregator>(eventAggr);
		}

		#region WordWrap

		private bool mWordWrap = false;

		/// <summary>
		///Toggle state WordWrap
		/// </summary>
		public bool WordWrap
		{
			get
			{
				return this.mWordWrap;
			}
			set
			{
				if (this.mWordWrap != value)
				{
					this.mWordWrap = value;
					this.RaisePropertyChanged("WordWrap");
				}
			}
		}

		#endregion WordWrap

		#region ShowLineNumbers

		private bool _ShowLineNumbers = false;

		/// <summary>
		///Toggle state WordWrap
		/// </summary>
		public bool ShowLineNumbers
		{
			get
			{
				return this._ShowLineNumbers;
			}
			set
			{
				if (this._ShowLineNumbers != value)
				{
					this._ShowLineNumbers = value;
					this.RaisePropertyChanged("ShowLineNumbers");
				}
			}
		}

		#endregion ShowLineNumbers

		#region ToggleEditorOptionCommand

		private DelegateCommand<object> _toggleEditorOptionCommand = null;

		public ICommand ToggleEditorOptionCommand
		{
			get
			{
				if (this._toggleEditorOptionCommand == null)
				{
					this._toggleEditorOptionCommand = new DelegateCommand<object>((p) => OnToggleEditorOption(p),
																	   (p) => CanToggleEditorOption(p));
				}

				return this._toggleEditorOptionCommand;
			}
		}

		private bool CanToggleEditorOption(object parameter)
		{
			//if (this.ActiveDocument != null)
			//	return true;

			//return false;
			return true;
		}

		private void OnToggleEditorOption(object parameter)
		{
			//FileViewModel f = this.ActiveDocument;

			if (parameter == null)
				return;

			if ((parameter is ToggleEditorOption) == false)
				return;

			ToggleEditorOption t = (ToggleEditorOption)parameter;

			switch (t)
			{
				case ToggleEditorOption.WordWrap:
					WordWrap = !WordWrap;
					break;

				case ToggleEditorOption.ShowLineNumber:
					ShowLineNumbers = !ShowLineNumbers;
					break;

				default:
					break;
			}
		}

		#endregion ToggleEditorOptionCommand
	}
}
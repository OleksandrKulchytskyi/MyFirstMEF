using FirstPrismApp.Infrastructure;
using FirstPrismApp.Infrastructure.Base;
using FirstPrismApp.Infrastructure.Services;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Utils;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace FullViewModule
{
	public sealed class FullViewViewModel : ViewModelBase, IFullViewViewModel, IDisposable
	{
		private readonly string[] _extensions = new string[4] { "txt", "log", "log4net", "dat" };

		private int disposed = 0;
		private GenericWeakReference<IUnityContainer> _container = null;
		private GenericWeakReference<IEventAggregator> _eventMgr = null;

		public FullViewViewModel(IFullView view, IEventAggregator eventAgg, IUnityContainer cont)
			: base(view)
		{
			_container = new GenericWeakReference<IUnityContainer>(cont);
			_eventMgr = new GenericWeakReference<IEventAggregator>(eventAgg);

			if (_container.IsAlive)
				ToolbarViewModel = _container.Get().Resolve<IToolbarViewModel>();

			if ((this.View as IWindow) != null)
			{
				(View as IWindow).Loaded += FullViewViewModel_Loaded;
				(View as IWindow).Closing += FullViewViewModel_Closing;
			}
		}

		private void FullViewViewModel_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			Dispose();
		}

		private void FullViewViewModel_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			IStateService state = _container.Get().Resolve<IStateService>();
			if (state != null && !string.IsNullOrEmpty(state.GetCurrentDocument()))
				DocumentPath = state.GetCurrentDocument();
		}

		#region Property ToolbarViewModel
		private IToolbarViewModel _ToolbarViewModel;
		public IToolbarViewModel ToolbarViewModel
		{
			get
			{
				return _ToolbarViewModel;
			}
			set
			{
				if (_ToolbarViewModel != value)
				{
					_ToolbarViewModel = value;
					RaisePropertyChanged("ToolbarViewModel");
				}
			}
		}
		#endregion



		#region Property IsDirty
		private bool _IsDirty;

		public bool IsDirty
		{
			get
			{
				return _IsDirty;
			}
			set
			{
				if (_IsDirty != value)
				{
					_IsDirty = value;
					RaisePropertyChanged("IsDirty");
				}
			}
		}

		#endregion Property IsDirty

		#region Property DocumentPath

		private string _DocumentPath;

		public string DocumentPath
		{
			get
			{
				return _DocumentPath;
			}
			set
			{
				if (_DocumentPath != value)
				{
					_DocumentPath = value;
					RaisePropertyChanged("DocumnetPath");
					if (File.Exists(_DocumentPath))
					{
						this._document = new TextDocument();
						string ext = Path.GetExtension(_DocumentPath);
						if (_extensions.Any(x => ext.IndexOf(x, StringComparison.OrdinalIgnoreCase) != -1))
						{
							using (Stream s = typeof(FullViewModule).Assembly.GetManifestResourceStream("FullViewModule.Highlightings.Default.xshd"))
							{
								using (var reader = new System.Xml.XmlTextReader(s))
								{
									HighlightDef = ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader.Load(reader, HighlightingManager.Instance);
								}
							}
						}
						using (FileStream fs = new FileStream(this._DocumentPath, FileMode.Open, FileAccess.Read, FileShare.Read))
						{
							using (StreamReader reader = FileReader.OpenStream(fs, Encoding.UTF8))
							{
								Document = new TextDocument(reader.ReadToEnd());
							}
						}
						ContentId = _DocumentPath;
					}
				}
			}
		}

		#endregion Property DocumentPath

		#region ContentId

		private string _contentId = null;

		public string ContentId
		{
			get { return _contentId; }
			set
			{
				if (_contentId != value)
				{
					_contentId = value;
					RaisePropertyChanged("ContentId");
				}
			}
		}

		#endregion ContentId

		#region TextContent

		private TextDocument _document = null;

		public TextDocument Document
		{
			get { return this._document; }
			set
			{
				if (this._document != value)
				{
					this._document = value;
					RaisePropertyChanged("Document");
					IsDirty = true;
				}
			}
		}

		#endregion TextContent

		#region DocumentName

		public string DocumentName
		{
			get
			{
				if (DocumentPath == null)
					return "Noname" + (IsDirty ? "*" : "");

				return System.IO.Path.GetFileName(DocumentPath) + (IsDirty ? "*" : "");
			}
		}

		#endregion DocumentName

		#region HighlightingDefinition

		private IHighlightingDefinition _highlightdef = null;

		public IHighlightingDefinition HighlightDef
		{
			get { return this._highlightdef; }
			set
			{
				if (this._highlightdef != value)
				{
					this._highlightdef = value;
					RaisePropertyChanged("HighlightDef");
					IsDirty = true;
				}
			}
		}

		#endregion HighlightingDefinition

		#region Title

		/// <summary>
		/// Title is the string that is usually displayed - with or without dirty mark '*' - in the docking environment
		/// </summary>
		public string Title
		{
			get
			{
				return System.IO.Path.GetFileName(this.DocumentPath) + (this.IsDirty == true ? "*" : string.Empty);
			}
		}

		#endregion Title

		public void Dispose()
		{
			if (disposed == 1) return;
			disposed = 1;

			if (_eventMgr != null) _eventMgr = null;
			if (_container != null) _container = null;
			(View as IWindow).Loaded -= FullViewViewModel_Loaded;
			(View as IWindow).Closing -= FullViewViewModel_Closing;
		}
	}
}
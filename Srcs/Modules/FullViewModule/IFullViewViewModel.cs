using FirstPrismApp.Infrastructure;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;

namespace FullViewModule
{
	public interface IFullViewViewModel : IViewModel
	{
		bool IsDirty { get; set; }

		string Title { get; }

		string DocumentName { get; }

		string DocumentPath { get; set; }

		TextDocument Document { get; set; }

		string ContentId { get; set; }

		IHighlightingDefinition HighlightDef { get; set; }
	}
}
using Microsoft.Practices.Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Infrastructure.Events
{
	public sealed class CloseDocumentEvent : CompositePresentationEvent<CloseDocumentEvent>
	{
		public string PathToDocument { get; set; }
	}
}

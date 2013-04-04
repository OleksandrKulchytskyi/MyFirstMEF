using Microsoft.Practices.Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Infrastructure.Events
{
	public class DocumentLoadingEvent : CompositePresentationEvent<DocumentLoadingEvent>
	{
		public string Path { get; set; }
	}
}

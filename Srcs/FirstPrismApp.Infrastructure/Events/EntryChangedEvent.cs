using Business.Common;
using Microsoft.Practices.Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Infrastructure.Events
{
	public sealed class EntryChangedEvent : CompositePresentationEvent<EntryChangedEvent>
	{
		public LogItem SelectedItem { get; set; }
	}
}

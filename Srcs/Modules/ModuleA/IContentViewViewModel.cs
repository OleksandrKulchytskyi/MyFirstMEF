using FirstPrismApp.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleA
{
	public interface IContentViewViewModel : IViewModel
	{
		string Message { get; set; }
	}
}

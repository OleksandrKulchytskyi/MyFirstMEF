using FirstPrismApp.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FullViewModule
{
	public interface IFullViewViewModel:IViewModel
	{
		string DocumentPath { get; set; }
	}
}

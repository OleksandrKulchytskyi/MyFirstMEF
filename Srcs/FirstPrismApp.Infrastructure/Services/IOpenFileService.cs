using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FirstPrismApp.Infrastructure.Services
{
	public interface IOpenFileService
	{
		string Open(object location = null);
		string OpenFromID(string contentID);
	}
}

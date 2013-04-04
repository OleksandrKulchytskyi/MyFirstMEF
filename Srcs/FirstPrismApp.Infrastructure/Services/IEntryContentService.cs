using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Infrastructure.Services
{
	public interface IEntryContentService
	{
		string GetErrorContentForLine(string file,int line);
	}
}

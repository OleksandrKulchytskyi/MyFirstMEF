using Core.Infrastructure.Base;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Infrastructure.Services
{
	public sealed class WindowProviderService : IWindowProvider
	{
		private IWindowMapper _mapper;

		public WindowProviderService(IUnityContainer container)
		{
			_mapper = container.Resolve<IWindowMapper>();
		}

		public IWindow Generate(string scope)
		{
			Type windType = _mapper.Get(scope);
			if (windType != null)
				return Activator.CreateInstance(windType) as IWindow;
			else
				return default(IWindow);
		}
	}
}

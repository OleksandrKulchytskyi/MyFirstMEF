using FirstPrismApp.Infrastructure.Base;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FirstPrismApp.Infrastructure.Services
{
	public sealed class WindowMapper : IWindowMapper
	{
		private readonly ConcurrentDictionary<string, Type> _container;

		public WindowMapper()
		{
			_container = new ConcurrentDictionary<string,Type>(StringComparer.OrdinalIgnoreCase);
		}

		public void Map(string scope, Type windType)
		{
			if (_container.ContainsKey(scope))
				throw new InvalidOperationException("Mapping already exists.");

			_container.TryAdd(scope, windType);
		}

		public void UnMap(string scope)
		{
			Type windType;
			_container.TryRemove(scope, out windType);
		}

		public void Clear()
		{
			_container.Clear();
		}

		public Type Get(string scope)
		{
			Type wind;
			if (_container.TryGetValue(scope, out wind))
				return wind;
			return null;
		}
	}
}

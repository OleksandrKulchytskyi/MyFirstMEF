using System;

namespace Core.Infrastructure.Base
{
	public sealed class GenericWeakReference<T>
	{
		private WeakReference _weak = null;

		public GenericWeakReference(T obj)
			: this(obj, false)
		{
		}

		public GenericWeakReference(T obj, bool trackResurrection)
		{
			if (obj == null)
				throw new ArgumentNullException("obj");
			_weak = new WeakReference(obj, trackResurrection);
		}

		public T Get()
		{
			return (T)_weak.Target;
		}

		public bool IsAlive
		{
			get
			{
				return _weak.IsAlive;
			}
		}
	}
}
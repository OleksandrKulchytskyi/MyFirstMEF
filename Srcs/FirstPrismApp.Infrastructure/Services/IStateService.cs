using System.Collections.Generic;
using System.Linq;

namespace FirstPrismApp.Infrastructure.Services
{
	public interface IStateService
	{
		string GetCurrentDocument();

		IList<string> GetRecent();

		void AddToRecentAndSetCurrent(string doc);
	}

	public sealed class ApplicationStateService : IStateService
	{
		private Queue<string> _container;

		public ApplicationStateService()
		{
			_container = new Queue<string>(8);
		}

		public string GetCurrentDocument()
		{
			if (_container.Count == 0)
				return null;
			return _container.Peek();
		}

		public IList<string> GetRecent()
		{
			return _container.ToList();
		}

		public void AddToRecentAndSetCurrent(string doc)
		{
			if (_container.Count == 10)
				_container.Dequeue();

			_container.Enqueue(doc);
		}
	}
}
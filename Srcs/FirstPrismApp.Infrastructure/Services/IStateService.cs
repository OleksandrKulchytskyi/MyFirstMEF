using System.Collections.Generic;
using System.Linq;

namespace FirstPrismApp.Infrastructure.Services
{
	public interface IStateService
	{
		string GetCurrentDocument();

		IList<string> GetRecent();

		void AddToRecentAndSetCurrent(string doc);
		void CloseDocument();
	}

	public sealed class ApplicationStateService : IStateService
	{
		private Queue<string> _container;
		private string _currentDoc = null;
		public ApplicationStateService()
		{
			_container = new Queue<string>(8);
		}

		public string GetCurrentDocument()
		{
			return _currentDoc;
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
			_currentDoc = doc;
		}


		public void CloseDocument()
		{
			_currentDoc = null;
		}
	}
}
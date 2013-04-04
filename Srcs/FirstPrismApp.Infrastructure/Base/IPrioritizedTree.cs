using System.Collections.ObjectModel;

namespace Core.Infrastructure.Base
{
	public interface IPrioritizedTree<T>
	{
		bool Add(T item);

		bool Remove(string key);

		T Get(string key);

		ReadOnlyObservableCollection<T> Children { get; }

		int Priority { get; }

		string Key { get; }
	}
}
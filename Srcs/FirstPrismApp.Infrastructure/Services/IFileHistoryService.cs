using Core.Infrastructure.Helpers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Infrastructure.Services
{
	public interface IFileHistoryService
	{
		event EventHandler<RecentEventArgs> RecentChanged;

		void InitializeFromFile();

		IEnumerable<string> GetRecent();

		bool AddToRecent(string pathToFile);

		void Clear();

		Task UpdateStorage();
	}

	public enum RecentAction
	{
		None = 0,
		Added,
		Removed
	}

	[Serializable]
	public class RecentEventArgs : EventArgs
	{
		public RecentAction Action { get; set; }

		public string Item { get; set; }
	}

	public sealed class FileHistoryService : IFileHistoryService
	{
		private readonly string _storage;
		private readonly string _basePath;
		private readonly ConcurrentStack<string> _container;

		public FileHistoryService()
		{
			_storage = "recent.dat";
			_basePath = Path.GetDirectoryName(typeof(FileHistoryService).Assembly.Location);
			_container = new ConcurrentStack<string>();
			//InitializeFromFile();
		}

		private EventHandler<RecentEventArgs> _recentChanged;
		public event EventHandler<RecentEventArgs> RecentChanged
		{
			add { _recentChanged += value.MakeWeak(eh => _recentChanged -= eh); }
			remove { }
		}

		private void OnRecentChanged(string item, RecentAction action)
		{
			if (_recentChanged != null)
				_recentChanged(this, new RecentEventArgs { Action = action, Item = item });
		}

		public void InitializeFromFile()
		{
			if (!string.IsNullOrEmpty(_basePath))
			{
				string file = Directory.EnumerateFileSystemEntries(_basePath, "*.dat").FirstOrDefault(x => Path.GetFileName(x).Equals(_storage));
				if (!string.IsNullOrEmpty(file))
				{
					_container.Clear();
					using (StreamReader sr = new StreamReader(file, System.Text.Encoding.UTF8))
					{
						string line = null;
						while ((line = sr.ReadLine()) != null)
						{
							if (string.IsNullOrEmpty(line))
								continue;
							_container.Push(line);
							OnRecentChanged(line, RecentAction.Added);
						}
					}
				}
			}
		}

		public IEnumerable<string> GetRecent()
		{
			return _container.ToArray();
		}

		public bool AddToRecent(string pathToFile)
		{
			if (_container.Contains(pathToFile))
				return false;

			string Value;
			if (_container.Count == 8)
				if (_container.TryPop(out Value))
					OnRecentChanged(Value, RecentAction.Removed);

			_container.Push(pathToFile);
			OnRecentChanged(pathToFile, RecentAction.Added);
			return true;
		}

		public void Clear()
		{
			_container.Clear();
		}

		public Task UpdateStorage()
		{
			return Task.Factory.StartNew(UpdateInternal);
		}

		private void UpdateInternal()
		{
			string fPath = Path.Combine(_basePath, _storage);

			if (File.Exists(fPath))
				File.Delete(fPath);

			using (StreamWriter sw = new StreamWriter(fPath, false, System.Text.Encoding.UTF8))
			{
				foreach (string item in _container.ToArray())
				{
					sw.WriteLine(item);
				}
			}
		}
	}
}
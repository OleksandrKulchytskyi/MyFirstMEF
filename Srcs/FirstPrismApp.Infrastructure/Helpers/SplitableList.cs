using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Core.Infrastructure.Helpers
{
	public sealed class SplitableList<T> : IEnumerable<T>
	{
		private class BucketSlot<TItem> : IEnumerable<TItem>
		{
			private readonly int _threshold = 10000;
			private TItem[] _bucket;
			private int _fromIndx;
			private int _currIndx;

			public BucketSlot()
			{
				_currIndx = -1;
				_bucket = new TItem[_threshold];
			}

			public BucketSlot(int fromIndex)
				: this()
			{
				_fromIndx = fromIndex;
			}

			public int Count
			{
				get
				{
					return _currIndx + 1;
				}
			}

			public int CurrentGlobalPointerIndex
			{
				get
				{
					return _fromIndx + _currIndx;
				}
			}

			public int FromGlobalIndex
			{
				get
				{
					return _fromIndx;
				}
			}

			public TItem this[int index]
			{
				get
				{
					if (index < 0 || index >= _threshold)
						throw new IndexOutOfRangeException();
					return _bucket[index];
				}
				set
				{
					if (index < 0 || index >= _threshold)
						throw new IndexOutOfRangeException();
					_bucket[index] = value;
				}
			}

			public TItem[] Data
			{
				get
				{
					return _bucket;
				}
			}

			public bool CanAdd
			{
				get
				{
					return (_currIndx < (_threshold - 1));
				}
			}

			public void Add(TItem item)
			{
				if (!CanAdd)
					throw new InvalidOperationException("Bucket is full.Adding feature is unvaliable.");
				_bucket[Interlocked.Increment(ref _currIndx)] = item;
			}

			public IEnumerator<TItem> GetEnumerator()
			{
				for (int i = 0; i < _currIndx + 1; i++)
				{
					yield return _bucket[i];
				}
			}

			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			public TItem GetFromGlobalIndx(int index)
			{
				if (IsInRange(index))
					return this[index - _fromIndx];
				return default(TItem);
			}

			public void SetFromGlobalIndx(int index, TItem val)
			{
				if (IsInRange(index))
					this[index - _fromIndx] = val;
			}

			public bool IsInRange(int index)
			{
				return ((index >= _fromIndx) && (index <= (_fromIndx + _threshold - 1)));
			}
		}

		private int _capacity = 1;
		private readonly int _bucketCount;
		private List<BucketSlot<T>> _buckets;
		int _currentBucket = 0;
		int _currentIndx = -1;

		public SplitableList()
		{
			_bucketCount = 10000;
			_buckets = new List<BucketSlot<T>>();
			_buckets.Add(new BucketSlot<T>());
		}

		public SplitableList(int capacity)
		{
			_capacity = capacity;
			_bucketCount = 10000;
			double partLength = Math.Floor((double)(capacity / _bucketCount));
			int parts = (int)partLength;
			_buckets = new List<BucketSlot<T>>((int)partLength);
			for (int i = 0; i < partLength; i++)
				_buckets.Add(null);
		}

		public int BucketsCount
		{
			get
			{
				return _buckets.Count;
			}
		}

		public void Add(T item)
		{
		Begin:
			if (_buckets[_currentBucket] == null)
				_buckets[_currentBucket] = new BucketSlot<T>(_currentIndx == -1 ? ++_currentIndx : _currentIndx);

			if (_buckets[_currentBucket].CanAdd)
			{
				_buckets[_currentBucket].Add(item);
				Interlocked.Increment(ref _currentIndx);
			}
			else
			{
				if (_buckets.Count == _currentBucket + 1)
				{
					_buckets.Add(new BucketSlot<T>(_currentIndx));
					_buckets[Interlocked.Increment(ref _currentBucket)].Add(item);
					Interlocked.Increment(ref _currentIndx);
				}
				else
				{
					Interlocked.Increment(ref _currentBucket);
					goto Begin;
				}
			}
		}

		public int Count
		{
			get { return (_currentIndx + 1); }
		}

		public IEnumerator<T> GetEnumerator()
		{
			return (_buckets.GetEnumerator() as IEnumerator<T>);
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return _buckets.GetEnumerator();
		}

		public List<T> GetRange(int fromIndx, int count)
		{
			if (Count < (fromIndx + count))
				throw new InvalidOperationException("Fail to retrieve such items.");

			List<T> items = new List<T>(count);
			bool stop = false;
			int currentIndx = fromIndx;
			int fromBucket = GetBucketIndxForElementIndex(fromIndx);
			for (int i = fromBucket; i <= _bucketCount && !stop; i++)
			{
				while (_buckets[i].IsInRange(currentIndx))
				{
					items.Add(_buckets[i].GetFromGlobalIndx(currentIndx));
					currentIndx++;
					if ((currentIndx - fromIndx) == count)
					{
						stop = true;
						break;
					}
				}
			}

			return items;
		}

		public T this[int indx]
		{
			get
			{
				if (indx < 0 || indx >= Count)
					throw new IndexOutOfRangeException();
				int bucketIndx = GetBucketIndxForElementIndex(indx);
				return _buckets[bucketIndx].GetFromGlobalIndx(indx);
			}
			set
			{
				if (indx < 0 || indx >= Count)
					throw new IndexOutOfRangeException();
				int bucketIndx = GetBucketIndxForElementIndex(indx);
				_buckets[bucketIndx].SetFromGlobalIndx(indx, value);
			}
		}

		private int GetBucketIndxForElementIndex(int index)
		{
			for (int i = 0; i < _buckets.Count; i++)
			{
				if (index >= _buckets[i].FromGlobalIndex &&
					index <= (_buckets[i].FromGlobalIndex + (_bucketCount - 1)))
					return i;
			}
			throw new IndexOutOfRangeException();
		}
	}
}

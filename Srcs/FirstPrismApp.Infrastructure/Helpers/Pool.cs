﻿using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Core.Infrastructure.Helpers
{
	/// <summary>
	/// Provides a thread safely pool of re-usable objects.
	/// </summary>
	/// <typeparam name="T">Type of stored objects</typeparam>
	public abstract class Pool<T>
	{
		/// <summary>
		/// concurrent stack object
		/// </summary>
		private readonly ConcurrentStack<PoolSlot<T>> _storage; // storing objects "in pool"
		/// <summary>
		/// lock free semaphore
		/// </summary>
		private readonly LockFreeSemaphore _allocSemaphore; // light semaphore for allocate operations

		private int _currentCount;

		/// <summary>
		/// Initializes a new instance of the <see cref="Pool{T}"/> with specified upper limit.
		/// </summary>
		/// <param name="maxCapacity"></param>
		protected Pool(int maxCapacity)
		{
			if (maxCapacity < 1)
				throw new ArgumentOutOfRangeException("maxCapacity", "Max capacity must be greater than 0");

			_storage = new ConcurrentStack<PoolSlot<T>>();
			_allocSemaphore = new LockFreeSemaphore(maxCapacity, maxCapacity);
		}

		#region Public members

		/// <summary>
		/// Gets the current number of the available <see cref="T"/> in pool.
		/// </summary>
		public int CurrentCount
		{
			get { return _currentCount; }
		}

		/// <summary>
		/// Gets the number of the <see cref="T"/>, ever created in pool.
		/// </summary>
		public int TotalCount
		{
			get { return _allocSemaphore.MaxCount - _allocSemaphore.CurrentCount; }
		}

		/// <summary>
		/// Gets available object's slot from pool or creates new one.
		/// </summary>
		/// <returns>Pool slot</returns>
		public PoolSlot<T> TakeSlot()
		{
			PoolSlot<T> slot;
			if (TryPop(out slot))
				return slot;
			if (TryAllocatePop(out slot))
				return slot;
			return WaitPop();
		}

		/// <summary>
		/// Puts the object's slot back to the pool.
		/// </summary>
		/// <param name="slot">The slot to return</param>
		/// <exception cref="ArgumentNullException" />
		/// <exception cref="ArgumentException" />
		/// <exception cref="InvalidOperationException" />
		public void Release(PoolSlot<T> slot)
		{
			if (slot == null)
				throw new ArgumentNullException("slot");
			if (slot.GetStatus(this))
				throw new InvalidOperationException("Specified object is already in the pool");

			CleanUp(slot.Object);
			Push(slot);
		}

		/// <summary>
		/// Attempts to reduce the number of allocated objects.
		/// </summary>
		/// <param name="targetTotalCount">The target minimum number of objects</param>
		/// <returns>true, if the target minimum number of objects was reached, otherwise, false</returns>
		public bool TryReduceTotal(int targetTotalCount)
		{
			if (targetTotalCount < 0)
				throw new ArgumentOutOfRangeException("targetTotalCount");

			var removingCount = TotalCount - targetTotalCount;
			for (var i = 0; i < removingCount; i++)
				if (!TryPopRemove())
					return TotalCount == targetTotalCount;
			return TotalCount == targetTotalCount;
		}

		/// <summary>
		/// Waits for the pool to releasing all objects.
		/// Ensures that all objects are release before returning.
		/// </summary>
		public void WaitAll()
		{
			while (_currentCount != TotalCount)
				Wait();
		}

		public override string ToString()
		{
			return string.Format("{0}: {1}/{2}/{3}", GetType().Name, _currentCount,
								 TotalCount, _allocSemaphore.MaxCount);
		}

		#endregion Public members

		#region Pool operations

		/// <summary>
		/// Attempts to create and adds the specified number of instances of <see cref="T"/> to the pool.
		/// </summary>
		/// <param name="count">Count of objects to add</param>
		/// <returns>true if the operation was successfull, otherwise, false</returns>
		protected bool TryAllocatePush(int count)
		{
			for (var i = 0; i < count; i++)
				if (!TryAllocatePush())
					return false;
			return true;
		}

		/// <summary>
		/// Attempts to create and adds a new instance of <see cref="T"/> to the pool.
		/// </summary>
		/// <returns>true if the operation was successfull, otherwise, false</returns>
		protected bool TryAllocatePush()
		{
			if (_allocSemaphore.TryTake())
			{
				Push(Allocate());
				return true;
			}
			return false;
		}

		/// <summary>
		/// Attempts to create, register with status "Out of pool" and return a new instance of <see cref="T"/>
		/// </summary>
		/// <param name="slot"></param>
		/// <returns>true if the operation was successfully, otherwise, false</returns>
		protected bool TryAllocatePop(out PoolSlot<T> slot)
		{
			if (_allocSemaphore.TryTake())
			{
				slot = Allocate();
				return true;
			}

			slot = null;
			return false;
		}

		protected bool TryPopRemove()
		{
			PoolSlot<T> slot;
			if (TryPop(out slot))
			{
				_allocSemaphore.Release();
				return true;
			}
			return false;
		}

		/// <summary>
		/// Waits for a free slot
		/// </summary>
		/// <returns>Pool slot</returns>
		protected PoolSlot<T> WaitPop()
		{
			PoolSlot<T> slot;
			while (!TryPop(out slot))
				Wait();
			return slot;
		}

		/// <summary>
		/// Provides a delay for other pool operations
		/// </summary>
		protected void Wait()
		{
			if (!Thread.Yield())
				Thread.Sleep(100);
		}

		private PoolSlot<T> Allocate()
		{
			var obj = ObjectConstructor();
			var slot = new PoolSlot<T>(obj, this);
			HoldSlotInObject(obj, slot);
			return slot;
		}

		#endregion Pool operations

		#region For overriding

		/// <summary>
		/// Initializes a new object, ready to be placed in the pool
		/// </summary>
		/// <returns>The initialized object</returns>
		protected abstract T ObjectConstructor();

		protected virtual void HoldSlotInObject(T @object, PoolSlot<T> slot)
		{
		}

		/// <summary>
		/// Provides clean up of the object before returning to the pool
		/// </summary>
		/// <param name="object">Objects</param>
		protected virtual void CleanUp(T @object)
		{
		}

		#endregion For overriding

		#region Storage wrappers

		private void Push(PoolSlot<T> slot)
		{
			slot.SetStatus(true);
			_storage.Push(slot);
			Interlocked.Increment(ref _currentCount);
		}

		private bool TryPop(out PoolSlot<T> slot)
		{
			if (_storage.TryPop(out slot))
			{
				Interlocked.Decrement(ref _currentCount);
				slot.SetStatus(false);
				return true;
			}
			slot = null;
			return false;
		}

		#endregion Storage wrappers
	}
}
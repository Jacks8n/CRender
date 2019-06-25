using System;
using System.Runtime.CompilerServices;

using static CUtility.Extension.MarshalExt;

namespace CUtility.Collection
{
    public unsafe class UnsafeList<T> : UnsafeCollection<T> where T : unmanaged
    {
        public override T this[int index]
        {
#if DEBUG
            get => index > -1 && index < Capacity ? _itemsPtr[index] : throw new IndexOutOfRangeException();
            set => _itemsPtr[index > -1 && index < Capacity ? index : throw new IndexOutOfRangeException()] = value;
#elif RELEASE
            get => _itemsPtr[index];
            set => _itemsPtr[index] = value;
#endif
        }

        public override int Count => _lengthPtr[0];

        public int Capacity { get; private set; }

        public UnsafeList(int capacity = 4)
        {
            _itemsPtr = Alloc<T>(capacity);
            _lengthPtr = Alloc<int>(1);
            _lengthPtr[0] = 0;
            Capacity = capacity;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(T item)
        {
            if (Count == Capacity)
                ExtendCapacity(Capacity);
            _itemsPtr[_lengthPtr[0]++] = item;
        }

        /// <summary>
        /// The new items added are not guaranteed to be default(T)
        /// </summary>
        public void AddEmpty(int count)
        {
            _lengthPtr[0] += count;
            if (_lengthPtr[0] > Capacity)
                _lengthPtr[0] = Capacity;
        }

        public void Clear()
        {
            _lengthPtr[0] = 0;
        }

        public T Pop()
        {
            if (Count < 1)
                throw new Exception();
            return this[--_lengthPtr[0]];
        }

        private void ExtendCapacity(int amount)
        {
            _itemsPtr = ReAlloc(_itemsPtr, Count + amount);
            Capacity += amount;
        }
    }
}

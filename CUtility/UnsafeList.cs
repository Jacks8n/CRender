using System;
using System.Runtime.CompilerServices;

using static CUtility.Extension.MarshalExt;

namespace CUtility.Collection
{
    public unsafe class UnsafeList<T> : UnsafeCollection<T> where T : unmanaged
    {
        private const int DEFAULT_CAPACITY = 4;

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

        public UnsafeList(int capacity = DEFAULT_CAPACITY)
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
                AdjustCapacity(Capacity + Capacity);
            _itemsPtr[_lengthPtr[0]++] = item;
        }

        public void AddRange<TCollection>(TCollection collection) where TCollection : UnsafeCollection<T>
        {
            if (collection.Count + Count > Capacity)
                AdjustCapacity(collection.Count + Count);
            Assign(Count, collection.GetPointer(), collection.Count);
            _lengthPtr[0] += collection.Count;
        }

        public void EnsureVacant(int count)
        {
            if (count < Capacity)
                return;
            if (count > Capacity + Capacity)
                AdjustCapacity(count);
            else
                AdjustCapacity(Capacity + Capacity);
        }

        public T* ArchivePointer()
        {
            Clear();
            _itemsPtr = Alloc<T>(DEFAULT_CAPACITY);
            Capacity = 0;
            T* ptr = _itemsPtr;
            return ptr;
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

        private void AdjustCapacity(int capacity)
        {
            _itemsPtr = ReAlloc(_itemsPtr, capacity);
            if (capacity > Capacity)
                for (int i = Count; i < capacity; i++)
                    _itemsPtr[i] = default;
            Capacity = capacity;
        }
    }
}

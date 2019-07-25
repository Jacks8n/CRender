using System;
using System.Runtime.CompilerServices;

using static CUtility.Extension.MarshalExt;

namespace CUtility.Collection
{
    public abstract unsafe class UnsafeCollection<T> : IDisposable where T : unmanaged
    {
        public abstract T this[int index] { get; set; }

        public abstract int Count { get; }

        protected T* _itemsPtr;

        protected int* _lengthPtr;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Swap(int l, int r)
        {
            T temp = _itemsPtr[l];
            _itemsPtr[l] = _itemsPtr[r];
            _itemsPtr[r] = temp;
        }

        public T* GetPointer(int offset = 0)
        {
            return _itemsPtr + offset;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Assign(int index, T item)
        {
            this[index] = item;
        }

        public void Assign<TItem>(int index, TItem item) where TItem : unmanaged
        {
            *(TItem*)(_itemsPtr + index) = item;
        }

        public void Assign(int index, T[] items)
        {
            for (int i = 0; i < items.Length; i++)
                this[index++] = items[i];
        }

        public void Assign<TItem>(int index, TItem[] items) where TItem : unmanaged
        {
            TItem* ptr = (TItem*)(_itemsPtr + index);
            for (int i = 0; i < items.Length; i++)
                ptr[i] = items[i];
        }

        public void Assign(int index, T* items, int count)
        {
            while (--count >= 0)
                _itemsPtr[index + count] = items[count];
        }

        public TItem Read<TItem>(int index) where TItem : unmanaged
        {
            return *(TItem*)(_itemsPtr + index);
        }

        public void Read<TItem>(int index, TItem* ptr) where TItem : unmanaged
        {
            *ptr = Read<TItem>(index);
        }

        public void Read<TItem>(int from, int to, TItem[] results, int resultFrom = 0) where TItem : unmanaged
        {
            TItem* ptr = (TItem*)(_itemsPtr + from);
            while (--to >= from)
                results[resultFrom++] = ptr[to];
        }

        public void Read(UnsafeCollection<T> results)
        {
            Read(0, Count - 1, results);
        }

        public void Read(int from, int to, UnsafeCollection<T> results, int resultFrom = 0)
        {
            while (to >= from)
                results[resultFrom++] = _itemsPtr[to--];
        }

        public void Reverse()
        {
            Reverse(0, Count);
        }

        public void Reverse(int from, int length)
        {
            for (int i = length / 2 - 1; i > -1; i--)
                Swap(i + from, length - 1 - i + from);
        }

        public void Dispose()
        {
            if (Count > 0 && _itemsPtr[0] is IDisposable)
                for (int i = 0; i < Count; i++)
                    ((IDisposable)this[i]).Dispose();

            Free(_itemsPtr);
            Free(_lengthPtr);
        }
    }
}

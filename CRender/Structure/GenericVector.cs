using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CRender.Structure
{
    public unsafe struct GenericVector<T> : IEnumerable<T>, IDisposable where T : unmanaged
    {
        public T X => _values[0];
        public T Y => _values[1];
        public T Z => _values[2];
        public T W => _values[3];

        public T R => X;
        public T G => Y;
        public T B => Z;
        public T A => W;

        public GenericVector<T> RG => new GenericVector<T>(_values, 0, 2);
        public GenericVector<T> RGB => new GenericVector<T>(_values, 0, 3);

        public GenericVector<T> XY => RG;
        public GenericVector<T> XYZ => RGB;

        public T this[int index] { get => _values[index]; set => _values[index] = value; }

        public int Length { get; }

        private readonly T* _values;

        private int _addableIndex;

        public GenericVector(int size)
        {
            _values = (T*)Marshal.AllocHGlobal(sizeof(T) * size);
            Length = size;
            _addableIndex = 0;
            for (int i = 0; i < Length; i++)
                _values[i] = default;
        }

        private GenericVector(T* arr, int start, int count)
        {
            _values = arr + start;
            Length = count;
            _addableIndex = count;
        }

        public void Write(GenericVector<T> other)
        {
            for (int i = 0; i < Length && i < other.Length; i++)
                _values[i] = other._values[i];
            _addableIndex = other.Length;
        }

        public void Add(T item)
        {
            if (_addableIndex < Length)
                _values[_addableIndex++] = item;
        }

        void IDisposable.Dispose()
        {
            Marshal.FreeHGlobal((IntPtr)_values);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;

using static CRender.MarshalExt;

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

        public static int SizeOf()
        {
            return sizeof(int) * 2 + sizeof(T*);
        }

        public GenericVector(int size)
        {
            _values = Alloc<T>(size);
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

        public void Write(T value)
        {
            for (int i = 0; i < Length; i++)
                _values[i] = value;
        }

        public void Write(GenericVector<T> other)
        {
            for (int i = 0; i < Length && i < other.Length; i++)
                _values[i] = other._values[i];
            _addableIndex = other.Length;
        }

        public void Clear()
        {
            _addableIndex = 0;
            for (int i = 0; i < Length; i++)
                _values[i] = default;
        }

        public void Add(T item)
        {
            if (_addableIndex < Length)
                _values[_addableIndex++] = item;
        }

        public static GenericVector<T> operator +(GenericVector<T> l, GenericVector<T> r)
        {
            if (typeof(T) == typeof(float))
                for (int i = 0; i < l.Length; i++)
                    l._values[i] = (T)(object)((float)(object)l._values[i] + (float)(object)r._values[i]);
            else if (typeof(T) == typeof(int))
                for (int i = 0; i < l.Length; i++)
                    l._values[i] = (T)(object)((int)(object)l._values[i] + (int)(object)r._values[i]);
            return l;
        }

        public static GenericVector<T> operator -(GenericVector<T> l, GenericVector<T> r)
        {
            if (typeof(T) == typeof(float))
                for (int i = 0; i < l.Length; i++)
                    l._values[i] = (T)(object)((float)(object)l._values[i] - (float)(object)r._values[i]);
            else if (typeof(T) == typeof(int))
                for (int i = 0; i < l.Length; i++)
                    l._values[i] = (T)(object)((int)(object)l._values[i] - (int)(object)r._values[i]);
            return l;
        }

        public static GenericVector<T> operator *(GenericVector<T> l, GenericVector<T> r)
        {
            if (typeof(T) == typeof(float))
                for (int i = 0; i < l.Length; i++)
                    l._values[i] = (T)(object)((float)(object)l._values[i] * (float)(object)r._values[i]);
            else if (typeof(T) == typeof(int))
                for (int i = 0; i < l.Length; i++)
                    l._values[i] = (T)(object)((int)(object)l._values[i] * (int)(object)r._values[i]);
            return l;
        }

        public static GenericVector<T> operator /(GenericVector<T> l, GenericVector<T> r)
        {
            if (typeof(T) == typeof(float))
                for (int i = 0; i < l.Length; i++)
                    l._values[i] = (T)(object)((float)(object)l._values[i] / (float)(object)r._values[i]);
            else if (typeof(T) == typeof(int))
                for (int i = 0; i < l.Length; i++)
                    l._values[i] = (T)(object)((int)(object)l._values[i] / (int)(object)r._values[i]);
            return l;
        }

        void IDisposable.Dispose()
        {
            Free(_values);
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
using System;
using System.Collections;
using System.Collections.Generic;

using static CUtility.Extension.MarshalExt;

namespace CUtility.Math
{
    public unsafe struct GenericVector<T> : IEnumerable<T>, IDisposable where T : unmanaged
    {
        public T X => ElementsPtr[0];
        public T Y => ElementsPtr[1];
        public T Z => ElementsPtr[2];
        public T W => ElementsPtr[3];

        public T R => X;
        public T G => Y;
        public T B => Z;
        public T A => W;

        public GenericVector<T> RG => new GenericVector<T>(ElementsPtr, 0, 2);
        public GenericVector<T> RGB => new GenericVector<T>(ElementsPtr, 0, 3);

        public GenericVector<T> XY => RG;
        public GenericVector<T> XYZ => RGB;

        public T this[int index] { get => ElementsPtr[index]; set => ElementsPtr[index] = value; }

        public int Length { get; }

        public readonly T* ElementsPtr;

        private int _addableIndex;

        public GenericVector(int size)
        {
            ElementsPtr = Alloc<T>(size);
            Length = size;
            _addableIndex = 0;
            for (int i = 0; i < Length; i++)
                ElementsPtr[i] = default;
        }

        private GenericVector(T* arr, int start, int count)
        {
            ElementsPtr = arr + start;
            Length = count;
            _addableIndex = count;
        }

        public void Write(T value)
        {
            for (int i = 0; i < Length; i++)
                ElementsPtr[i] = value;
        }

        public void Write(GenericVector<T> other)
        {
            for (int i = 0; i < Length && i < other.Length; i++)
                ElementsPtr[i] = other.ElementsPtr[i];
            _addableIndex = other.Length;
        }

        public void Add(T item)
        {
            if (_addableIndex < Length)
                ElementsPtr[_addableIndex++] = item;
        }

        public void Clear()
        {
            _addableIndex = 0;
            for (int i = 0; i < Length; i++)
                ElementsPtr[i] = default;
        }

        public static GenericVector<T> operator +(GenericVector<T> l, GenericVector<T> r)
        {
            if (typeof(T) == typeof(float))
                for (int i = 0; i < l.Length; i++)
                    l.ElementsPtr[i] = (T)(object)((float)(object)l.ElementsPtr[i] + (float)(object)r.ElementsPtr[i]);
            else if (typeof(T) == typeof(int))
                for (int i = 0; i < l.Length; i++)
                    l.ElementsPtr[i] = (T)(object)((int)(object)l.ElementsPtr[i] + (int)(object)r.ElementsPtr[i]);
            return l;
        }

        public static GenericVector<T> operator -(GenericVector<T> l, GenericVector<T> r)
        {
            if (typeof(T) == typeof(float))
                for (int i = 0; i < l.Length; i++)
                    l.ElementsPtr[i] = (T)(object)((float)(object)l.ElementsPtr[i] - (float)(object)r.ElementsPtr[i]);
            else if (typeof(T) == typeof(int))
                for (int i = 0; i < l.Length; i++)
                    l.ElementsPtr[i] = (T)(object)((int)(object)l.ElementsPtr[i] - (int)(object)r.ElementsPtr[i]);
            return l;
        }

        public static GenericVector<T> operator *(GenericVector<T> l, GenericVector<T> r)
        {
            if (typeof(T) == typeof(float))
                for (int i = 0; i < l.Length; i++)
                    l.ElementsPtr[i] = (T)(object)((float)(object)l.ElementsPtr[i] * (float)(object)r.ElementsPtr[i]);
            else if (typeof(T) == typeof(int))
                for (int i = 0; i < l.Length; i++)
                    l.ElementsPtr[i] = (T)(object)((int)(object)l.ElementsPtr[i] * (int)(object)r.ElementsPtr[i]);
            return l;
        }

        public static GenericVector<T> operator /(GenericVector<T> l, GenericVector<T> r)
        {
            if (typeof(T) == typeof(float))
                for (int i = 0; i < l.Length; i++)
                    l.ElementsPtr[i] = (T)(object)((float)(object)l.ElementsPtr[i] / (float)(object)r.ElementsPtr[i]);
            else if (typeof(T) == typeof(int))
                for (int i = 0; i < l.Length; i++)
                    l.ElementsPtr[i] = (T)(object)((int)(object)l.ElementsPtr[i] / (int)(object)r.ElementsPtr[i]);
            return l;
        }

        void IDisposable.Dispose()
        {
            Free(ElementsPtr);
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
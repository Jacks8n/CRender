using System;
using System.Collections;
using System.Collections.Generic;

using static CUtility.Extension.MarshalExt;

namespace CUtility.Math
{
    public unsafe struct GenericVector<T> : IEnumerable<float>, IEnumerable<Vector2>, IDisposable where T : unmanaged
    {
        public T X => ElementsPtr[0];
        public T Y => ElementsPtr[1];
        public T Z => ElementsPtr[2];
        public T W => ElementsPtr[3];

        public T R => X;
        public T G => Y;
        public T B => Z;
        public T A => W;

        public GenericVector<T> RG => new GenericVector<T>(ElementsPtr, 2);
        public GenericVector<T> RGB => new GenericVector<T>(ElementsPtr, 3);

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

        private GenericVector(T* arr, int count)
        {
            ElementsPtr = arr;
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

        public void Clear()
        {
            _addableIndex = 0;
            for (int i = 0; i < Length; i++)
                ElementsPtr[i] = default;
        }

        public static GenericVector<T> operator +(GenericVector<T> l, GenericVector<T> r)
        {
            for (int i = 0; i < l.Length; i++)
                if (typeof(T) == typeof(float))
                    l.ElementsPtr[i] = (T)(object)((float)(object)l.ElementsPtr[i] + (float)(object)r.ElementsPtr[i]);
                else if (typeof(T) == typeof(int))
                    l.ElementsPtr[i] = (T)(object)((int)(object)l.ElementsPtr[i] + (int)(object)r.ElementsPtr[i]);
            return l;
        }

        public static GenericVector<T> operator -(GenericVector<T> l, GenericVector<T> r)
        {
            for (int i = 0; i < l.Length; i++)
                if (typeof(T) == typeof(float))
                    l.ElementsPtr[i] = (T)(object)((float)(object)l.ElementsPtr[i] - (float)(object)r.ElementsPtr[i]);
                else if (typeof(T) == typeof(int))
                    l.ElementsPtr[i] = (T)(object)((int)(object)l.ElementsPtr[i] - (int)(object)r.ElementsPtr[i]);
            return l;
        }

        public static GenericVector<T> operator *(GenericVector<T> l, GenericVector<T> r)
        {
            for (int i = 0; i < l.Length; i++)
                if (typeof(T) == typeof(float))
                    l.ElementsPtr[i] = (T)(object)((float)(object)l.ElementsPtr[i] * (float)(object)r.ElementsPtr[i]);
                else if (typeof(T) == typeof(int))
                    l.ElementsPtr[i] = (T)(object)((int)(object)l.ElementsPtr[i] * (int)(object)r.ElementsPtr[i]);
            return l;
        }

        public static GenericVector<T> operator /(GenericVector<T> l, GenericVector<T> r)
        {
            for (int i = 0; i < l.Length; i++)
                if (typeof(T) == typeof(float))
                    l.ElementsPtr[i] = (T)(object)((float)(object)l.ElementsPtr[i] / (float)(object)r.ElementsPtr[i]);
                else if (typeof(T) == typeof(int))
                    l.ElementsPtr[i] = (T)(object)((int)(object)l.ElementsPtr[i] / (int)(object)r.ElementsPtr[i]);
            return l;
        }

        public void Add(T item)
        {
            if (_addableIndex < Length)
                ElementsPtr[_addableIndex++] = item;
            else
                throw new Exception();
        }

        public void Add(Vector2 item)
        {
            if (typeof(T) != typeof(float))
                throw new Exception();
            Add((T)(object)item.X);
            Add((T)(object)item.Y);
        }

        public void Add(Vector3 item)
        {
            if (typeof(T) != typeof(float))
                throw new Exception();
            Add((T)(object)item.X);
            Add((T)(object)item.Y);
            Add((T)(object)item.Z);
        }

        public void Add(Vector4 item)
        {
            if (typeof(T) != typeof(float))
                throw new Exception();
            Add((T)(object)item.X);
            Add((T)(object)item.Y);
            Add((T)(object)item.Z);
            Add((T)(object)item.W);
        }

        void IDisposable.Dispose()
        {
            Free(ElementsPtr);
        }

        #region Not Implemented

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator<float> IEnumerable<float>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator<Vector2> IEnumerable<Vector2>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
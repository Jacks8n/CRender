namespace CUtility.Collection
{
    public unsafe partial class UnsafeArray<T> : UnsafeCollection<T> where T : unmanaged
    {
        public override T this[int index]
        {
#if DEBUG
            get => index > -1 && index < Count ? _itemsPtr[index] : throw new System.IndexOutOfRangeException();
            set => _itemsPtr[index > -1 && index < Count ? index : throw new System.IndexOutOfRangeException()] = value;
#else
            get => _itemsPtr[index];
            set => _itemsPtr[index] = value;
#endif
        }

        public T this[int x, int y]
        {
            get => this[x + y * _lengthPtr[1]];
            set => this[x + y * _lengthPtr[1]] = value;
        }

        public override int Count { get => _lengthPtr[0]; protected set => _lengthPtr[0] = value; }

        public int LengthInBytes => Count * sizeof(T);

        public int Dimension { get; private set; }

        public UnsafeArray(int length0)
        {
            Resize(length0);
        }

        public UnsafeArray(int length0, int length1)
        {
            Resize(length0, length1);
        }

        public UnsafeArray(T[] arr) : this(arr.Length)
        {
            Assign(0, arr);
        }

        public int GetLength(int dimension)
        {
            return dimension < 1 || dimension > Dimension ?
                -1 : _lengthPtr[Dimension == 1 ? 0 : dimension];
        }
    }
}
using CUtility.Extension;

namespace CUtility.Collection
{
    public unsafe struct UnsafeBlock<T> where T : unmanaged
    {
        public T this[int index] { get => Pointer[index]; set => Pointer[index] = value; }

        public T* Pointer { get; private set; }

        public int Length { get; private set; }

        public UnsafeBlock(int length)
        {
            Pointer = MarshalExtension.Alloc<T>(length);
            Length = length;
        }

        public void Free()
        {
            MarshalExtension.Free(Pointer);
        }
    }
}

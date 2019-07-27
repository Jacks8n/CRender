namespace CUtility.StupidWrapper
{
    // Thanks to the stupid limitation that high dimension pointer can't
    // be taken as "unmanaged" in .NET Core 2.2 and previous versions
    // Thank you CLR :D

    public unsafe struct FloatPointer
    {
        public float* Pointer;

        public FloatPointer(float* ptr)
        {
            Pointer = ptr;
        }

        public static implicit operator FloatPointer(float* ptr)
        {
            return new FloatPointer(ptr);
        }
    }
}

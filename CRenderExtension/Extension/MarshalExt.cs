using System;
using System.Runtime.InteropServices;

namespace CUtility.Extension
{
    public unsafe static class MarshalExt
    {
        public static T* Alloc<T>(int length) where T : unmanaged
        {
            return (T*)Marshal.AllocHGlobal(sizeof(T) * length);
        }

        public static void ReAlloc(void* ptr, int length)
        {
            Marshal.ReAllocHGlobal((IntPtr)ptr, (IntPtr)length);
        }

        public static void Free(void* ptr)
        {
            Marshal.FreeHGlobal((IntPtr)ptr);
        }
    }
}

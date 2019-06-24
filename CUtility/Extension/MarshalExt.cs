using System;
using System.Runtime.InteropServices;

namespace CUtility.Extension
{
    public unsafe static class MarshalExt
    {
        public static T* Alloc<T>(int length = 1) where T : unmanaged
        {
            return AllocBytes<T>(sizeof(T) * length);
        }

        public static T* AllocBytes<T>(int byteCount) where T : unmanaged
        {
            return byteCount < 1 ? null : (T*)Marshal.AllocHGlobal(byteCount);
        }

        public static T* ReAlloc<T>(T* ptr, int length) where T : unmanaged
        {
            return (T*)Marshal.ReAllocHGlobal((IntPtr)ptr, (IntPtr)(sizeof(T) * length));
        }

        public static void Free(void* ptr)
        {
            if (ptr == null)
                return;
            Marshal.FreeHGlobal((IntPtr)ptr);
        }
    }
}

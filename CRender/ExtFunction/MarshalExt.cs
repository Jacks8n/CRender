using System;
using System.Runtime.InteropServices;

namespace CRender
{
    public unsafe static class MarshalExt
    {
        public static T* Alloc<T>(int length) where T : unmanaged
        {
            return (T*)Marshal.AllocHGlobal(sizeof(T) * length);
        }

        public static void Free<T>(T* ptr) where T : unmanaged
        {
            Marshal.FreeHGlobal((IntPtr)ptr);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CUtility.Extension
{
    public unsafe static class MarshalExtension
    {
        private static List<IntPtr> _ptrsToFree = new List<IntPtr>();

        static MarshalExtension()
        {
            ConsoleEvent.OnCtrlClose += () =>
              {
                  for (int i = 0; i < _ptrsToFree.Count; i++)
                      Marshal.FreeHGlobal(_ptrsToFree[i]);
              };
        }

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

        public static void Move<T>(T* from, T* to, int length = 1) where T : unmanaged
        {
            while (--length >= 0)
                *to = *from;
        }

        public static void Free(void* ptr)
        {
            if (ptr == null)
                return;
            Marshal.FreeHGlobal((IntPtr)ptr);
        }

        public static void FreeWhenExit(void* ptr)
        {
            if (ptr == null)
                return;
            _ptrsToFree.Add((IntPtr)ptr);
        }
    }
}

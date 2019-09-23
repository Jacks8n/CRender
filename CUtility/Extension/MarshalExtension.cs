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
            AppDomain.CurrentDomain.DomainUnload += (sender, args) =>
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
            return (T*)Marshal.AllocHGlobal(byteCount);
        }

        public static T* AllocPermanant<T>(int length = 1) where T : unmanaged
        {
            return AllocBytesPermanent<T>(sizeof(T) * length);
        }

        public static T* AllocBytesPermanent<T>(int byteCount) where T : unmanaged
        {
            T* ptr = AllocBytes<T>(byteCount);
            FreeWhenExit(ptr);
            return ptr;
        }

        public static T* ReAlloc<T>(T* ptr, int length) where T : unmanaged
        {
            return (T*)Marshal.ReAllocHGlobal((IntPtr)ptr, (IntPtr)(sizeof(T) * length));
        }

        public static void Move<T>(T* from, T* to, int length = 1) where T : unmanaged
        {
            while (--length >= 0)
                to[length] = from[length];
        }

        public static void Set<T0, T1>(T0* to, T1 value, int length = 1) where T0 : unmanaged where T1 : unmanaged
        {
            T1* ptr = (T1*)to;
            while (--length > -1)
                ptr[length] = value;
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

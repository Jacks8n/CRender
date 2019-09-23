using System;
using System.Collections.Generic;
using System.Reflection;

namespace CShader
{
    public static unsafe class SizeOfHelper
    {
        private static readonly Dictionary<Type, int> SizeDictionary = new Dictionary<Type, int>();

        public static int SizeOfPointer => sizeof(IntPtr);

        public static int SizeOf(Type type)
        {
            if (type == typeof(byte))
                return sizeof(byte);
            else if (type == typeof(char))
                return sizeof(char);
            else if (type == typeof(ushort))
                return sizeof(ushort);
            else if (type == typeof(short))
                return sizeof(short);
            else if (type == typeof(int))
                return sizeof(int);
            else if (type == typeof(uint))
                return sizeof(uint);
            else if (type == typeof(long))
                return sizeof(long);
            else if (type == typeof(ulong))
                return sizeof(ulong);
            else if (type == typeof(float))
                return sizeof(float);
            else if (type == typeof(double))
                return sizeof(double);
            else if (type == typeof(decimal))
                return sizeof(decimal);
            else if (type.IsPointer)
                return SizeOfPointer;
            else if (type.IsValueType)
            {
                if (SizeDictionary.TryGetValue(type, out int size))
                    return size;

                size = 0;
                FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
                for (int i = 0; i < fields.Length; i++)
                    size += SizeOf(fields[i].FieldType);
                SizeDictionary.Add(type, size);
                return size;
            }

            throw new Exception($"Can't get the size of {type}");
        }
    }
}
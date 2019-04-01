using System;

namespace CShader
{
    public static class SizeOfHelper
    {
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

            throw new Exception($"Can't get the size of {type}");
        }
    }
}

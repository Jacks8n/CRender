using System;
using System.Runtime.InteropServices;

namespace CRender
{
    public static class ConsoleExt
    {
        private struct _COORD
        {
            public readonly short X, Y;

            public _COORD(short x, short y)
            {
                X = x;
                Y = y;
            }
        }

        /// <summary>
        /// https://docs.microsoft.com/en-us/windows/console/getstdhandle
        /// </summary>
        private const int STD_OUTPUT_HANDLE = -11;

        private static readonly IntPtr HANDLE_OUTPUT = GetStdHandle(STD_OUTPUT_HANDLE);

        private static readonly _COORD COORD_ZERO = new _COORD(0, 0);

        public static uint Output(char[] value)
        {
            WriteConsoleOutputCharacter(HANDLE_OUTPUT, value, (uint)value.Length, COORD_ZERO, out uint charsWritten);
            return charsWritten;
        }

        /// <summary>
        /// https://docs.microsoft.com/en-us/windows/console/getstdhandle
        /// </summary>
        [DllImport("Kernel32.dll")]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        /// <summary>
        /// https://docs.microsoft.com/en-us/windows/console/writeconsoleoutputcharacter
        /// </summary>
        [DllImport("Kernel32.dll")]
        private static extern bool WriteConsoleOutputCharacter(IntPtr hConsoleOutput, char[] lpCharacter, uint nLength, _COORD dwWriteCoord, out uint lpNumberOfCharsWritten);

    }
}

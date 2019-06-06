using System;
using System.Runtime.InteropServices;

namespace CUtility.Extension
{
    public class ConsoleExt : JSingleton<ConsoleExt>, IDisposable
    {
        private struct _COORD
        {
            public short X, Y;

            public _COORD(short x, short y)
            {
                X = x;
                Y = y;
            }
        }

        /// <summary>
        /// https://docs.microsoft.com/en-us/windows/console/getstdhandle
        /// </summary>
        private const uint GENERIC_WRITE = 0x40000000;

        /// <summary>
        /// https://docs.microsoft.com/en-us/windows/console/createconsolescreenbuffer
        /// </summary>
        private const uint FILE_SHARE_WRITE = 0x00000002;

        /// <summary>
        /// <para> https://docs.microsoft.com/en-us/windows/console/createconsolescreenbuffer </para>
        /// <para> But you can't find the value on doc site, which lies in wincon.h header file </para>
        /// </summary>
        private const uint CONSOLE_TEXTMODE_BUFFER = 0x00000001;

        private static readonly _COORD COORD_ZERO = new _COORD(0, 0);

        private readonly IntPtr _outputBuffer0, _outputBuffer1;

        public ConsoleExt()
        {
            _outputBuffer0 = CreateConsoleScreenBuffer(GENERIC_WRITE, FILE_SHARE_WRITE, IntPtr.Zero, CONSOLE_TEXTMODE_BUFFER, IntPtr.Zero);
            _outputBuffer1 = CreateConsoleScreenBuffer(GENERIC_WRITE, FILE_SHARE_WRITE, IntPtr.Zero, CONSOLE_TEXTMODE_BUFFER, IntPtr.Zero);
            SetConsoleActiveScreenBuffer(_outputBuffer1);
        }

        public static void Output(char[] value)
        {
            WriteToBufferAndShow(value, Instance._outputBuffer0);
            WriteToBufferAndShow(value, Instance._outputBuffer1);
        }

        private static void WriteToBufferAndShow(char[] value, IntPtr buffer)
        {
            WriteConsoleOutputCharacter(buffer, value, (uint)value.Length, COORD_ZERO, out _);
            SetConsoleActiveScreenBuffer(buffer);
        }

        void IDisposable.Dispose()
        {
            CloseHandle(_outputBuffer0);
            CloseHandle(_outputBuffer1);
        }

        /// <summary>
        /// https://docs.microsoft.com/en-us/windows/console/createconsolescreenbuffer
        /// </summary>
        [DllImport("Kernel32.dll")]
        private static extern IntPtr CreateConsoleScreenBuffer(uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwFlags, IntPtr lpScreenBufferData);

        /// <summary>
        /// https://docs.microsoft.com/en-us/windows/console/writeconsoleoutputcharacter
        /// </summary>
        [DllImport("Kernel32.dll")]
        private static extern bool WriteConsoleOutputCharacter(IntPtr hConsoleOutput, char[] lpCharacter, uint nLength, _COORD dwWriteCoord, out uint lpNumberOfCharsWritten);

        /// <summary>
        /// https://docs.microsoft.com/en-us/windows/console/setconsoleactivescreenbuffer
        /// </summary>
        [DllImport("Kernel32.dll")]
        private static extern bool SetConsoleActiveScreenBuffer(IntPtr hConsoleOutput);

        [DllImport("Kernel32.dll")]
        private static extern bool CloseHandle(IntPtr hObject);
    }
}

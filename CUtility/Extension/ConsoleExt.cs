using System;
using System.Runtime.InteropServices;

using static CUtility.Extension.MarshalExt;

namespace CUtility.Extension
{
    public class ConsoleExt
    {
        /// <summary>
        /// https://docs.microsoft.com/en-us/windows/console/coord-str
        /// </summary>
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
        /// https://docs.microsoft.com/en-us/windows/console/console-font-infoex
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct _CONSOLE_FONT_INFOEX
        {
            public uint cbSize;

            public uint nFont;

            public _COORD dwFontSize;

            public uint FontFamily;

            public uint FontWeight;

            /// <summary>
            /// <para> An array containing 32 chars </para>
            /// <para> The size is defined as LF_FACESIZE in wingdi.h </para>
            /// </summary>
            public long FaceName0, FaceName1, FaceName2, FaceName3, FaceName4, FaceName5, FaceName6, FaceName7;
        }

        /// <summary>
        /// https://docs.microsoft.com/en-us/windows/console/console-buffer-security-and-access-rights
        /// </summary>
        private const uint GENERIC_READ = 0x80000000;

        /// <summary>
        /// https://docs.microsoft.com/en-us/windows/console/console-buffer-security-and-access-rights
        /// </summary>
        private const uint GENERIC_WRITE = 0x40000000;

        private const uint GENERIC_READ_WRITE = GENERIC_READ | GENERIC_WRITE;

        /// <summary>
        /// https://docs.microsoft.com/en-us/windows/console/createconsolescreenbuffer
        /// </summary>
        private const uint FILE_SHARE_WRITE = 0x00000002;

        /// <summary>
        /// <para> https://docs.microsoft.com/en-us/windows/console/createconsolescreenbuffer </para>
        /// <para> But you can't find the value on the doc site, which lies in wincon.h header file </para>
        /// </summary>
        private const uint CONSOLE_TEXTMODE_BUFFER = 0x00000001;

        private const bool MAXIMUM_WINDOW = false;

        private static readonly _COORD COORD_ZERO = new _COORD(0, 0);

        private static readonly IntPtr _outputBuffer0, _outputBuffer1;

        private static readonly unsafe _CONSOLE_FONT_INFOEX* _consoleFontInfoExPtr;

        static unsafe ConsoleExt()
        {
            _outputBuffer0 = CreateConsoleScreenBuffer(GENERIC_READ_WRITE, FILE_SHARE_WRITE, IntPtr.Zero, CONSOLE_TEXTMODE_BUFFER, IntPtr.Zero);
            _outputBuffer1 = CreateConsoleScreenBuffer(GENERIC_READ_WRITE, FILE_SHARE_WRITE, IntPtr.Zero, CONSOLE_TEXTMODE_BUFFER, IntPtr.Zero);
            SetConsoleActiveScreenBuffer(_outputBuffer1);

            _consoleFontInfoExPtr = Alloc<_CONSOLE_FONT_INFOEX>();
            _consoleFontInfoExPtr->cbSize = (uint)sizeof(_CONSOLE_FONT_INFOEX);
            GetCurrentConsoleFontEx(_outputBuffer0, MAXIMUM_WINDOW, _consoleFontInfoExPtr);

            ConsoleEvent.OnCtrlClose += () =>
             {
                 Free(_consoleFontInfoExPtr);
                 CloseHandle(_outputBuffer0);
                 CloseHandle(_outputBuffer1);
             };
        }

        public static void Output(char[] value)
        {
            WriteToBufferAndShow(value, _outputBuffer0);
            WriteToBufferAndShow(value, _outputBuffer1);
        }

        public static unsafe void SetFontSize(short width, short height)
        {
            _consoleFontInfoExPtr->dwFontSize.X = width;
            _consoleFontInfoExPtr->dwFontSize.Y = height;
            SetCurrentConsoleFontEx(_outputBuffer0, bMaximumWindow: MAXIMUM_WINDOW, _consoleFontInfoExPtr);
            SetCurrentConsoleFontEx(_outputBuffer1, bMaximumWindow: MAXIMUM_WINDOW, _consoleFontInfoExPtr);
        }

        private static void WriteToBufferAndShow(char[] value, IntPtr buffer)
        {
            WriteConsoleOutputCharacter(buffer, value, (uint)value.Length, COORD_ZERO, out _);
            SetConsoleActiveScreenBuffer(buffer);
        }

#if DEBUG

        /// <summary>
        /// https://docs.microsoft.com/en-us/windows/desktop/api/errhandlingapi/nf-errhandlingapi-getlasterror
        /// </summary>
        /// <returns></returns>
        [DllImport("Kernel32.dll")]
        private static extern uint GetLastError();

#endif

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

        /// <summary>
        /// https://docs.microsoft.com/en-us/windows/console/getcurrentconsolefontex
        /// </summary>
        [DllImport("Kernel32.dll")]
        private static extern unsafe bool GetCurrentConsoleFontEx(IntPtr hConsoleOutput, bool bMaximumWindow, _CONSOLE_FONT_INFOEX* lpConsoleCurrentFont);

        /// <summary>
        /// https://docs.microsoft.com/en-us/windows/console/setcurrentconsolefontex
        /// </summary>
        [DllImport("Kernel32.dll")]
        private static extern unsafe bool SetCurrentConsoleFontEx(IntPtr hConsoleOutput, bool bMaximumWindow, _CONSOLE_FONT_INFOEX* lpConsoleCurrentFontEx);

        [DllImport("Kernel32.dll")]
        private static extern bool CloseHandle(IntPtr hObject);
    }
}

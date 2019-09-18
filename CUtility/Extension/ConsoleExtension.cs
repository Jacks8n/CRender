using System;
using System.Runtime.InteropServices;

using static CUtility.Extension.MarshalExtension;

namespace CUtility.Extension
{
    public static class ConsoleExtension
    {
        #region Windows
#if WINDOWS
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

        private const bool MAXIMUM_WINDOW = false;

        private const int STD_OUTPUT_HANDLE = -11;

        private static readonly unsafe _CONSOLE_FONT_INFOEX* _consoleFontInfoExPtr;

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

        /// <summary>
        /// https://docs.microsoft.com/en-us/windows/console/getstdhandle
        /// </summary>
        [DllImport("Kernel32.dll")]
        private static extern IntPtr GetStdHandle(int nStdHandle);

#endif
        #endregion

#if WINDOWS
        static unsafe ConsoleExtension()
        {
            _consoleFontInfoExPtr = Alloc<_CONSOLE_FONT_INFOEX>();
            _consoleFontInfoExPtr->cbSize = (uint)sizeof(_CONSOLE_FONT_INFOEX);
            GetCurrentConsoleFontEx(GetStdHandle(STD_OUTPUT_HANDLE), MAXIMUM_WINDOW, _consoleFontInfoExPtr);
            AppDomain.CurrentDomain.DomainUnload += (sender, args) => Free(_consoleFontInfoExPtr);
        }
#endif

        public static void Output(char[] value)
        {
            Console.SetCursorPosition(0, 0);
            Console.Write(value);
        }

        public static unsafe void SetFontSize(short width, short height)
        {
#if WINDOWS
            _consoleFontInfoExPtr->dwFontSize.X = width;
            _consoleFontInfoExPtr->dwFontSize.Y = width;
            SetCurrentConsoleFontEx(GetStdHandle(STD_OUTPUT_HANDLE), MAXIMUM_WINDOW, _consoleFontInfoExPtr);
#else
            Console.WriteLine("Failed: Setting font is not available on this platform");
#endif
        }
    }
}

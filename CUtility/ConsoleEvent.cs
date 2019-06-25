using System;
using System.Runtime.InteropServices;

namespace CUtility
{
    public static class ConsoleEvent
    {
        /// <summary>
        /// https://docs.microsoft.com/en-us/windows/console/handlerroutine
        /// </summary>
        private delegate bool HandlerRoutine(int dwCtrlType);

        /// <summary>
        /// https://docs.microsoft.com/en-us/windows/console/handlerroutine
        /// </summary>
        private const int CTRL_C_EVENT = 0;

        /// <summary>
        /// https://docs.microsoft.com/en-us/windows/console/handlerroutine
        /// </summary>
        private const int CTRL_BREAK_EVENT = 1;

        /// <summary>
        /// https://docs.microsoft.com/en-us/windows/console/handlerroutine
        /// </summary>
        private const int CTRL_CLOSE_EVENT = 2;

        /// <summary>
        /// https://docs.microsoft.com/en-us/windows/console/handlerroutine
        /// </summary>
        private const int CTRL_LOGOFF_EVENT = 5;

        /// <summary>
        /// https://docs.microsoft.com/en-us/windows/console/handlerroutine
        /// </summary>
        private const int CTRL_SHUTDOWN_EVENT = 6;

        public static event Action OnCtrlC, OnCtrlBreak, OnCtrlClose, OnLogoff, OnShutdown;

        static ConsoleEvent()
        {
            SetConsoleCtrlHandler(OnEvents, Add: true);
        }

        private static bool OnEvents(int type)
        {
            switch (type)
            {
                case CTRL_C_EVENT:
                    OnCtrlC?.Invoke();
                    break;
                case CTRL_BREAK_EVENT:
                    OnCtrlBreak?.Invoke();
                    break;
                case CTRL_CLOSE_EVENT:
                    OnCtrlClose?.Invoke();
                    break;
                case CTRL_LOGOFF_EVENT:
                    OnLogoff?.Invoke();
                    break;
                case CTRL_SHUTDOWN_EVENT:
                    OnShutdown?.Invoke();
                    break;
            }
            return false;
        }

        /// <summary>
        /// https://docs.microsoft.com/en-us/windows/console/setconsolectrlhandler
        /// </summary>
        [DllImport("Kernel32.dll")]
        private static extern bool SetConsoleCtrlHandler(HandlerRoutine HandlerRoutine, bool Add);

    }
}

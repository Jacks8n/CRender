using CRender.Structure;
using CUtility.Extension;

namespace CRender
{
    public static class CRenderer
    {
        public static float LastFrameElapsed { get; private set; } = 0;

        public static float CurrentSecond { get; private set; } = 0;

        private static long _currentRenderTick = 0;

        private static long _lastRenderTick = 0;

        private static readonly long _beginRenderTick = 0;

        private static int _frameCounts = 0;

        private static int _renderingBufferWidth = 0;

        static CRenderer()
        {
            _currentRenderTick = _beginRenderTick = System.DateTime.Now.Ticks;
        }

        public static void Render(CharRenderBuffer<float> buffer)
        {
            _lastRenderTick = _currentRenderTick;
            _currentRenderTick = System.DateTime.Now.Ticks;

            LastFrameElapsed = (_currentRenderTick - _lastRenderTick) * 1e-7f;
            CurrentSecond = (_currentRenderTick - _beginRenderTick) * 1e-7f;

            char[] outputChars = buffer.GetRenderBuffer();
            _renderingBufferWidth = buffer.Width;

            int cursorPos = _renderingBufferWidth - 1;
            if (CRenderSettings.IsCountFrames)
                cursorPos = DisplayString(outputChars, DisplayNumber(outputChars, cursorPos, _frameCounts) - 1, "Frame:") - 1;
            if (CRenderSettings.IsShowFPS)
                cursorPos = DisplayString(outputChars, DisplayNumber(outputChars, cursorPos, (int)(1f / LastFrameElapsed)) - 1, "FPS:") - 1;

            _frameCounts++;
            ConsoleExt.Output(outputChars);
        }

        public static void ResetFrameCount()
        {
            _frameCounts = 0;
        }

        private static int DisplayNumber(char[] buffer, int rightAlign, int number)
        {
            for (; number > 0; rightAlign--, number /= 10)
                buffer[rightAlign] = (char)(0x30 + (number % 10));
            return rightAlign;
        }

        private static int DisplayString(char[] buffer, int rightAlign, string str)
        {
            for (int i = str.Length - 1; i > -1; rightAlign--, i--)
                buffer[rightAlign] = str[i];
            return rightAlign;
        }
    }
}

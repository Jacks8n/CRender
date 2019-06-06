using System;
using CUtility.Math;
using CRender.Structure;
using CUtility;
using CUtility.Extension;

namespace CRender
{
    public static class CRenderer
    {
        public static float CurrentSecond { get; private set; }

        public static int DeltaMS { get; private set; }

        private static readonly JTimer _timer = new JTimer();

        private static int _frameCounts = 0;

        static CRenderer()
        {
            _timer.Start();
        }

        public static void UpdateRenderInfo()
        {
            DeltaMS = (int)_timer.DeltaMS;
            CurrentSecond += DeltaMS * .001f;
        }

        public static void Render(CharRenderBuffer<float> buffer)
        {
            char[] outputChars = buffer.GetRenderBuffer();
            int cursorPos = buffer.Width - 1;
            if (CRenderSettings.IsCountFrames)
                cursorPos = DisplayString(outputChars, DisplayNumber(outputChars, cursorPos, _frameCounts) - 1, "Frame:") - 1;
            if (CRenderSettings.IsShowFPS)
                cursorPos = DisplayString(outputChars, DisplayNumber(outputChars, cursorPos, JMath.RoundToInt(1000f / DeltaMS)) - 1, "FPS:") - 1;

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

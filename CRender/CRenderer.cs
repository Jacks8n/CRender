using CRender.Structure;

namespace CRender
{
    public static class CRenderer
    {
        private static int _frameCounts = 0;

        public static void Render(CharRenderBuffer<float> buffer)
        {
            char[] outputChars = buffer.CalculateColorChars();
            if (CRenderSettings.IsCountFrames)
            {
                _frameCounts++;
                for (int i = buffer.Width - 1, j = _frameCounts; j > 0; i--, j /= 10)
                    outputChars[i] = (char)(0x30 + (j % 10));
            }
            ConsoleExt.Output(outputChars);
        }

        public static void ResetFrameCount()
        {
            _frameCounts = 0;
        }
    }
}

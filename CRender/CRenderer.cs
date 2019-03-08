using System;
using CRender.Structure;
using CRender.Math;

using MathLib = System.Math;

namespace CRender
{
    public static class CRenderer
    {
        public static void Render(RenderBuffer<float> buffer)
        {
            char[] outputChar = new char[buffer.Width * buffer.Height];

            int outputIndex = 0;
            for (int j = 0; j < buffer.Height; j++)
                for (int i = 0; i < buffer.Width; i++)
                    outputChar[outputIndex++] = GetColorChar(buffer[i][j]);

            ConsoleExt.Output(outputChar);
        }

        public static void Clear()
        {
            Console.Clear();
        }

        private static readonly char[] COLOR_CHAR = { ' ', '.', '-', '+', '*', '#', '@' };

        private static readonly int COLOR_CHAR_COUNT = COLOR_CHAR.Length;

        private static char GetColorChar(GenericVector<float> color)
        {
            int index = (int)((color.R * .299f + color.G * .587f + color.B * .114f) * COLOR_CHAR_COUNT);
            return COLOR_CHAR[index == COLOR_CHAR_COUNT ? index - 1 : index];
        }
    }
}

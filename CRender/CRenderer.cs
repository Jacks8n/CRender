using System;
using CRender.Structure;
using CRender.Math;

using MathLib = System.Math;

namespace CRender
{
    public static class CRenderer
    {
        public static unsafe void Render(CharRenderBuffer<float> buffer)
        {
            ConsoleExt.Output(buffer.CalculateColorChars(), buffer.Width, buffer.Height);
        }
    }
}

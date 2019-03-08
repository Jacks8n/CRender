using System;
using CRender.Math;

namespace CRender
{
    public static class CRenderSettings
    {
        public static int RenderWidth => Console.WindowWidth;

        public static int RenderHeight => Console.WindowHeight;

        public static Vector2Int RenderSize => new Vector2Int(Console.WindowWidth, Console.WindowHeight);

        public static float AspectRatio => (float)Console.WindowWidth / Console.WindowHeight;
    }
}

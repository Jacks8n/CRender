using System;
using CUtility.Math;

namespace CRender
{
    public static class CRenderSettings
    {
        public static int RenderWidth => Console.WindowWidth;

        public static int RenderHeight => Console.WindowHeight;

        public static Vector2Int Resolution => new Vector2Int(Console.WindowWidth, Console.WindowHeight);

        public static Vector2 ResolutionF => new Vector2(Console.WindowWidth, Console.WindowHeight);

        public static bool IsCountFrames
        {
            get => _isCountFrames;
            set
            {
                if (_isCountFrames ^ value)
                    CRenderer.ResetFrameCount();
                _isCountFrames = value;
            }
        }

        public static bool IsShowFPS { get; set; }

        private static bool _isCountFrames;
    }
}

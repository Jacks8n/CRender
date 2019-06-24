using System;
using CUtility.Math;
using CUtility.Extension;

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

        public static void SetFontSize(short size) => ConsoleExt.SetFontSize(size, size);

        public static void SetFontSize(short width, short height) => ConsoleExt.SetFontSize(width, height);

        private static bool _isCountFrames;
    }
}

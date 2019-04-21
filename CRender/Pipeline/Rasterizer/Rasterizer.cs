using System;
using CUtility.Math;
using CRender.Structure;

using static CUtility.Extension.MarshalExt;

namespace CRender.Pipeline
{
    /// <summary>
    /// Rasterize primitives, based on the resolution passed in <see cref="StartRasterize(Vector2Int)"/>
    /// </summary>
    public sealed unsafe partial class Rasterizer : IDisposable
    {
        public static int RasterizeResultLength { get; private set; } = 0;

        private static bool _initialized = false;

        private static Vector2 _resolution = Vector2.Zero;

        private static Vector2 _discardInterval = Vector2.Zero;

        private static Vector2 _pixelSize = Vector2.One;

        private static Vector2Int* _rasterizeBufferPtr = null;

        private static Vector2* _pointsPtr = null;

        public static void StartRasterize(Vector2 resolution)
        {
            if (_initialized)
                throw new Exception("Rasterization has begun");
            _initialized = true;

            _resolution = resolution;
            _discardInterval = new Vector2(1e-2f / _resolution.X, 1e-2f / _resolution.Y);
            _pixelSize = new Vector2(1f / _resolution.X, 1f / _resolution.Y);
            _rasterizeBufferPtr = Alloc<Vector2Int>((int)resolution.X * (int)resolution.Y);
            RasterizeResultLength = 0;
        }

        public static void SetPoints(Vector2* pointsPtr)
        {
            _pointsPtr = pointsPtr;
        }

        public static int ContriveResult(Vector2Int* output)
        {
            for (int i = 0; i < RasterizeResultLength; i++)
                output[i] = _rasterizeBufferPtr[i];

            int temp = RasterizeResultLength;
            RasterizeResultLength = 0;
            return temp;
        }

        public static void ContriveResult(Vector2Int[] output, int start)
        {
            for (int i = 0; i < RasterizeResultLength; i++)
                output[start + i] = _rasterizeBufferPtr[i];
            RasterizeResultLength = 0;
        }

        public static Vector2Int[] ContriveResult()
        {
            Vector2Int[] result = new Vector2Int[RasterizeResultLength];
            ContriveResult(result, 0);
            return result;
        }

        public static void EndRasterize()
        {
            if (!_initialized)
                throw new Exception("Rasterization hasn't begun");
            _initialized = false;

            Free(_rasterizeBufferPtr);
        }

        /// <summary>
        /// Store result and shift <see cref="RasterizeResultLength"/>
        /// </summary>
        private static void OutputRasterization(Vector2Int value)
        {
            _rasterizeBufferPtr[RasterizeResultLength++] = value;
        }

        void IDisposable.Dispose()
        {
            if (_initialized)
                EndRasterize();
        }
    }
}

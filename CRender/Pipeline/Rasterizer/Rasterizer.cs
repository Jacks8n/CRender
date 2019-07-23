using System;
using CUtility.StupidWrapper;
using CUtility.Collection;
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
        public static int RasterizedPixelCount { get; private set; } = 0;

        private static bool _initialized = false;

        private static Vector2 _resolution = Vector2.Zero;

        private static Vector2 _pixelSize = Vector2.One;

        private static Vector2Int* _rasterizeBufferPtr = null;

        private static readonly UnsafeList<float> _fragmentDataPtr = new UnsafeList<float>();

        public static void StartRasterize(Vector2 resolution)
        {
            if (_initialized)
                throw new Exception("Rasterization has begun");
            _initialized = true;

            _resolution = resolution;
            _pixelSize.X = 1f / _resolution.X;
            _pixelSize.Y = 1f / _resolution.Y;
            _rasterizeBufferPtr = Alloc<Vector2Int>((int)resolution.X * (int)resolution.Y);
            RasterizedPixelCount = 0;
        }

        public static int ContriveResult(Vector2Int* output)
        {
            for (int i = 0; i < RasterizedPixelCount; i++)
                output[i] = _rasterizeBufferPtr[i];

            int temp = RasterizedPixelCount;
            RasterizedPixelCount = 0;
            return temp;
        }

        public static void ContriveResult(Vector2Int[] output, int start)
        {
            for (int i = 0; i < RasterizedPixelCount; i++)
                output[start + i] = _rasterizeBufferPtr[i];
            RasterizedPixelCount = 0;
        }

        public static Vector2Int[] ContriveResult()
        {
            Vector2Int[] result = new Vector2Int[RasterizedPixelCount];
            ContriveResult(result, 0);
            return result;
        }

        public static void EndRasterize()
        {
            if (!_initialized)
                throw new Exception("Rasterization hasn't begun");
            _initialized = false;

            Free(_rasterizeBufferPtr);
            _fragmentDataPtr.Clear();
        }

        /// <summary>
        /// Store result and shift <see cref="RasterizedPixelCount"/>
        /// </summary>
        private static void OutputRasterization(Vector2Int pixelCoord, UnsafeList<float> fragmentData)
        {
            _rasterizeBufferPtr[RasterizedPixelCount++] = pixelCoord;
            _fragmentDataPtr.AddRange(fragmentData);
        }

        void IDisposable.Dispose()
        {
            if (_initialized)
                EndRasterize();
        }
    }
}

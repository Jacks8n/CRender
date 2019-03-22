using System;
using System.Runtime.InteropServices;
using CRender.Math;
using CRender.Structure;

namespace CRender.Pipeline
{
    /// <summary>
    /// Rasterize primitives, based on the resolution passed in <see cref="StartRasterize(Vector2Int)"/>
    /// </summary>
    public sealed unsafe partial class Rasterizer : IDisposable
    {
        public int RasterzeResultLength => _rasterizeBufferUsed;

        /// <summary>
        /// Store result and shift <see cref="_rasterizeBufferUsed"/>
        /// </summary>
        private static Vector2Int OutputRasterization { set => _rasterizeBufferPtr[_rasterizeBufferUsed++] = value; }

        private static bool _initialized = false;

        private static Vector2 _resolution = Vector2.Zero;

        private static Vector2 _discardableInterval = Vector2.Zero;

        private static Vector2 _pixelSize = Vector2.One;

        private static Vector2Int* _rasterizeBufferPtr = null;

        private static int _rasterizeBufferUsed = 0;

        private static Vector2* _pointsPtr = null;

        public static void StartRasterize(Vector2 resolution)
        {
            if (_initialized)
                throw new Exception("Rasterization has begun");
            _initialized = true;

            _resolution = resolution;
            _discardableInterval = new Vector2(1e-3f / _resolution.X, 1e-3f / _resolution.Y);
            _pixelSize = new Vector2(1f / _resolution.X, 1f / _resolution.Y);
            _rasterizeBufferPtr = (Vector2Int*)Marshal.AllocHGlobal(sizeof(Vector2Int) * (int)resolution.X * (int)resolution.Y);
            _rasterizeBufferUsed = 0;
        }

        public static void SetPoints(Vector2* pointsPtr)
        {
            _pointsPtr = pointsPtr;
        }

        public static Vector2Int* ContriveResultPtr()
        {
            int i = 0;
            for (; i < _rasterizeBufferUsed; i++)
                outputPtr[i] = _rasterizeBufferPtr[i];
            return i;
        }

        public static void ContriveResult(Vector2Int[] output, int start)
        {
            for (int i = 0; i < _rasterizeBufferUsed; i++)
                output[start + i] = _rasterizeBufferPtr[i];
        }

        public static Vector2Int[] ContriveResult()
        {
            Vector2Int[] result = new Vector2Int[_rasterizeBufferUsed];
            ContriveResult(result, 0);
            return result;
        }

        public static void EndRasterize()
        {
            if (!_initialized)
                throw new Exception("Rasterization hasn't begun");
            _initialized = false;

            Marshal.FreeHGlobal((IntPtr)_rasterizeBufferPtr);
        }

        void IDisposable.Dispose()
        {
            if (_initialized)
                EndRasterize();
        }
    }
}

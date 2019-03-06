using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CRender.Math;
using CRender.Pipeline.Structure;
using CRender.Structure;

using MathLib = System.Math;

namespace CRender.Pipeline
{
    /// <summary>
    /// Rasterize primitives, based on the resolution passed in <see cref="StartRasterize(Vector2Int)"/>
    /// </summary>
    public static class Rasterizer
    {
        private static bool _initialized = false;

        private static Vector2 _resolution = Vector2.Zero;

        private static Vector2 _pixelSize = Vector2.Zero;

        private static unsafe Vector2* _pointsPtr;

        private static int _rasterizeBufferLength;

        private static unsafe Vector2Int* _rasterizeBufferPtr;

        private static int _rasterizeBufferUsed = 0;
        
        public static unsafe void StartRasterize(Vector2 resolution)
        {
            if (_initialized)
                throw new Exception("Rasterization has begun");
            _initialized = true;

            _rasterizeBufferUsed = 0;
            _resolution = resolution;
            _pixelSize = new Vector2(1f / resolution.X, 1f / resolution.Y);
            _rasterizeBufferLength = (int)resolution.X * (int)resolution.Y;
            _rasterizeBufferPtr = (Vector2Int*)Marshal.AllocHGlobal(sizeof(Vector2Int) * _rasterizeBufferLength);
        }

        public static unsafe void SetPoints(Vector2[] pointsPtr)
        {
            fixed (Vector2* ptr = &pointsPtr[0])
                _pointsPtr = ptr;
        }

        public static unsafe void ContriveResult(Vector2Int[] output, int start)
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

        public static unsafe void EndRasterize()
        {
            _initialized = false;
            Marshal.FreeHGlobal((IntPtr)_rasterizeBufferPtr);
        }

        public static unsafe void Line()
        {
            //0: X-major 1:Y-major
            int dir = MathLib.Abs(_pointsPtr[0].X - _pointsPtr[1].X) > MathLib.Abs(_pointsPtr[0].Y - _pointsPtr[1].Y) ? 0 : 1;

            float slope = (_pointsPtr[0][1 - dir] - _pointsPtr[1][1 - dir]) / (_pointsPtr[0][dir] - _pointsPtr[1][dir]) * _pixelSize[dir];

            int startIndex = _pointsPtr[0][dir] < _pointsPtr[1][dir] ? 0 : 1;

            float* startPoint = (float*)(_pointsPtr + startIndex);
            int end = (int)(_pointsPtr[1 - startIndex][dir] * _resolution.Y);

            Vector2Int resultPoint = new Vector2Int((int)(startPoint[0] * _resolution.X), (int)(startPoint[1] * _resolution.Y));
            for (float frac = 0; resultPoint[dir] < end; frac += _pixelSize[dir], resultPoint[dir]++)
            {
                if (frac >= slope)
                {
                    resultPoint[1 - dir]++;
                    frac = 0;
                }
                _rasterizeBufferPtr[_rasterizeBufferUsed++] = resultPoint;
            }
            _pointsPtr += 2;
        }
    }
}

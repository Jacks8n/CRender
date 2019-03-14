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

        private static unsafe Vector2Int* _rasterizeBufferPtr = null;

        private static int _rasterizeBufferUsed = 0;

        private static unsafe Vector2* _pointsPtr = null;

        public static unsafe void StartRasterize(Vector2 resolution)
        {
            if (_initialized)
                throw new Exception("Rasterization has begun");
            _initialized = true;

            _resolution = resolution;
            _rasterizeBufferPtr = (Vector2Int*)Marshal.AllocHGlobal(sizeof(Vector2Int) * (int)resolution.X * (int)resolution.Y);
            _rasterizeBufferUsed = 0;
        }

        public static unsafe void SetPoints(Vector2* pointsPtr)
        {
            _pointsPtr = pointsPtr;
        }

        public static unsafe void ContriveResult(Vector2Int[] output, int start)
        {
            for (int i = 0; i < _rasterizeBufferUsed; i++)
                output[start + i] = _rasterizeBufferPtr[i];
            _rasterizeBufferUsed = 0;
        }

        public static Vector2Int[] ContriveResult()
        {
            Vector2Int[] result = new Vector2Int[_rasterizeBufferUsed];
            ContriveResult(result, 0);
            return result;
        }

        public static unsafe void EndRasterize()
        {
            if (!_initialized)
                throw new Exception("Rasterization hasn't begun");
            _initialized = false;

            Marshal.FreeHGlobal((IntPtr)_rasterizeBufferPtr);
        }

        public static unsafe void Line()
        {
            if (_pointsPtr[0] == _pointsPtr[1])
                return;

            float xSub = (_pointsPtr[1].X - _pointsPtr[0].X) * _resolution.X,
                ySub = (_pointsPtr[1].Y - _pointsPtr[0].Y) * _resolution.Y;

            //0: X-major 1:Y-major
            int dir, otherDir;
            int dirStep, otherDirStep;
            float slopeAbs;
            if ((xSub > 0 ? xSub : -xSub) >= (ySub > 0 ? ySub : -ySub))
            {
                dir = 0;
                dirStep = xSub > 0 ? 1 : -1;
                otherDirStep = ySub > 0 ? 1 : -1;
                slopeAbs = ySub / xSub;
            }
            else
            {
                dir = 1;
                dirStep = ySub > 0 ? 1 : -1;
                otherDirStep = xSub > 0 ? 1 : -1;
                slopeAbs = xSub / ySub;
            }
            otherDir = 1 - dir;
            if (slopeAbs < 0)
                slopeAbs = -slopeAbs;

            Vector2Int resultPoint = new Vector2Int(JMath.RoundToInt(_pointsPtr[0].X * _resolution.X), JMath.RoundToInt(_pointsPtr[0].Y * _resolution.Y));
            if (resultPoint.X == _resolution.X)
                resultPoint.X--;
            if (resultPoint.Y == _resolution.Y)
                resultPoint.Y--;

            //End coordinate in Int
            int end = JMath.RoundToInt(_pointsPtr[1][dir] * _resolution[dir]);

            for (float otherDirFrac = slopeAbs; resultPoint[dir] != end; otherDirFrac += slopeAbs, resultPoint[dir] += dirStep)
            {
                _rasterizeBufferPtr[_rasterizeBufferUsed++] = resultPoint;
                if (otherDirFrac >= 1f)
                {
                    resultPoint[otherDir] += otherDirStep;
                    otherDirFrac--;
                }
            }
            _pointsPtr += 2;
        }
    }
}

using System;
using CUtility.Math;

namespace CRender.Pipeline
{
    public sealed unsafe partial class Rasterizer
    {
        public static void Line()
        {
            float xSub = (_pointsPtr[1].X - _pointsPtr[0].X) * _resolution.Y,
                ySub = (_pointsPtr[1].Y - _pointsPtr[0].Y) * _resolution.X;

            if (JMath.InRange(xSub, -_discardableInterval.X, _discardableInterval.X) &&
                JMath.InRange(ySub, -_discardableInterval.Y, _discardableInterval.Y))
                return;

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
            slopeAbs = MathF.Abs(slopeAbs);

            Vector2Int resultPoint = new Vector2Int(JMath.RoundToInt(_pointsPtr[0].X * _resolution.X), JMath.RoundToInt(_pointsPtr[0].Y * _resolution.Y));
            if (resultPoint.X == _resolution.X)
                resultPoint.X--;
            if (resultPoint.Y == _resolution.Y)
                resultPoint.Y--;

            //End coordinate in Int
            int end = JMath.RoundToInt(_pointsPtr[1][dir] * _resolution[dir]);

            for (float otherDirFrac = slopeAbs; resultPoint[dir] != end; otherDirFrac += slopeAbs, resultPoint[dir] += dirStep)
            {
                OutputRasterization = resultPoint;
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
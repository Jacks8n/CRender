using System;
using CUtility.Math;

namespace CRender.Pipeline
{
    public sealed unsafe partial class Rasterizer
    {
        public static void Line()
        {
            float xSub = (_pointsPtr[1].X - _pointsPtr[0].X) * _resolution.X,
                ySub = (_pointsPtr[1].Y - _pointsPtr[0].Y) * _resolution.Y;

            if (JMath.InRange(xSub, -_discardInterval.X, _discardInterval.X) &&
                JMath.InRange(ySub, -_discardInterval.Y, _discardInterval.Y))
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
            slopeAbs = slopeAbs < 0 ? -slopeAbs : slopeAbs;

            Vector2Int resultPoint = new Vector2Int(JMath.RoundToInt(_pointsPtr[0].X * _resolution.X), JMath.RoundToInt(_pointsPtr[0].Y * _resolution.Y));
            if (resultPoint.X == _resolution.X)
                resultPoint.X--;
            if (resultPoint.Y == _resolution.Y)
                resultPoint.Y--;

            //End coordinate in Int
            int end = JMath.RoundToInt(_pointsPtr[1][dir] * _resolution[dir]);

            for (float otherDirFrac = slopeAbs; resultPoint[dir] != end; otherDirFrac += slopeAbs, resultPoint[dir] += dirStep)
            {
                OutputRasterization(resultPoint);
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
using System;
using CUtility.Math;

namespace CRender.Pipeline
{
    public sealed unsafe partial class Rasterizer
    {
        private static readonly Interpolator LineInterpolator = new Interpolator();

        public static void Line(Vector2* verticesPtr, float** verticesDataPtr, int verticesDataCount)
        {
            Vector2 from = verticesPtr[0], to = verticesPtr[1];
            float xSub = (to.X - from.X) * _resolution.X,
                  ySub = (to.Y - from.Y) * _resolution.Y;

            //0: X-major 1:Y-major
            int dir;
            int dirStep, otherDirStep;
            float slopeAbs;
            if (Math.Abs(xSub) >= Math.Abs(ySub))
                dir = 0;
            else
            {
                dir = 1;
                float temp = xSub;
                xSub = ySub;
                ySub = temp;
            }
            dirStep = Math.Sign(xSub);
            otherDirStep = Math.Sign(ySub);
            slopeAbs = Math.Abs(ySub / xSub);

            bool ifInterpolate = verticesDataCount > 0;
            if (ifInterpolate)
                LineInterpolator.SetInterpolation(verticesDataPtr[0], verticesDataPtr[1], verticesDataCount, Math.Abs(xSub));

            Vector2Int resultPoint = new Vector2Int(JMath.RoundToInt(from.X * _resolution.X), JMath.RoundToInt(from.Y * _resolution.Y));
            if (resultPoint.X == _resolution.X)
                resultPoint.X--;
            if (resultPoint.Y == _resolution.Y)
                resultPoint.Y--;

            //End coordinate in Int
            int end = JMath.RoundToInt(to[dir] * _resolution[dir]);
            for (float otherDirFrac = slopeAbs; resultPoint[dir] != end; otherDirFrac += slopeAbs, resultPoint[dir] += dirStep)
            {
                if (ifInterpolate)
                {
                    OutputRasterization(resultPoint, LineInterpolator.InterpolatedValues);
                    LineInterpolator.IncrementStep();
                }
                else
                    OutputRasterization(resultPoint, LineInterpolator.InterpolatedValues);
                if (otherDirFrac >= 1f)
                {
                    resultPoint[1 - dir] += otherDirStep;
                    otherDirFrac--;
                }
            }
        }
    }
}
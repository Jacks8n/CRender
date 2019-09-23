using System;
using CUtility;
using CUtility.Math;

namespace CRender.Pipeline.Rasterization
{
    public sealed unsafe class Line : JSingleton<Line>, IPrimitiveRasterizer<LinePrimitive>
    {
        public Rasterizer.RasterizerEntry RasterizerEntry { get; }

        public Vector2 Resolution { private get; set; }

        private readonly Interpolator LineInterpolator = new Interpolator();

        public void Rasterize(LinePrimitive* linePtr)
        {
            Vector2* verticesPtr = linePtr->CoordsPtr;
            float** verticesDataPtr = linePtr->VerticesDataPtr;
            int verticesDataCount = linePtr->VerticesDataCount;
            Vector2 from = verticesPtr[0] * Resolution, to = verticesPtr[1] * Resolution;
            float xSub = to.X - from.X,
                  ySub = to.Y - from.Y;

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

            Vector2Int resultPoint = new Vector2Int(JMath.RoundToInt(from.X), JMath.RoundToInt(from.Y));
            if (resultPoint.X == Resolution.X)
                resultPoint.X--;
            if (resultPoint.Y == Resolution.Y)
                resultPoint.Y--;

            //End coordinate in Int
            int end = JMath.RoundToInt(to[dir]);
            for (float otherDirFrac = slopeAbs; resultPoint[dir] != end; otherDirFrac += slopeAbs, resultPoint[dir] += dirStep)
            {
                if (ifInterpolate)
                {
                    RasterizerEntry.OutputRasterization(resultPoint, LineInterpolator.InterpolatedValues);
                    LineInterpolator.IncrementStep();
                }
                else
                    RasterizerEntry.OutputRasterization(resultPoint, LineInterpolator.InterpolatedValues);
                if (otherDirFrac >= 1f)
                {
                    resultPoint[1 - dir] += otherDirStep;
                    otherDirFrac--;
                }
            }
        }
    }
}
using System;
using CUtility.Math;

using static CUtility.Math.JMathGeom;

namespace CRender.Pipeline
{
    public sealed unsafe partial class Rasterizer
    {
        public static void Triangle()
        {
            int topIndex = Highest(_verticesPtr[0], _verticesPtr[1], _verticesPtr[2]), midIndex;

            switch (topIndex)
            {
                case 0:
                    midIndex = LexicoCompareDownRight(_verticesPtr[1], _verticesPtr[2]) < 0 ? 1 : 2;
                    break;
                case 1:
                    midIndex = LexicoCompareDownRight(_verticesPtr[0], _verticesPtr[2]) + 1;
                    break;
                case 2:
                    midIndex = LexicoCompareDownRight(_verticesPtr[0], _verticesPtr[1]) < 0 ? 0 : 1;
                    break;
                default:
                    midIndex = 1;
                    break;
            }

            Vector2 top = _verticesPtr[topIndex] * _resolution, mid = _verticesPtr[midIndex] * _resolution, bottom = _verticesPtr[3 - topIndex - midIndex] * _resolution;
            if (JMath.Approx(top.Y, mid.Y))
                HorizontalEdgeTriangle(bottom, top.X, mid.X, top.Y);
            else if (JMath.Approx(mid.Y, bottom.Y))
                HorizontalEdgeTriangle(top, mid.X, bottom.X, mid.Y);
            else
            {
                float midSectionX = JMath.Lerp(JMath.Ratio(mid.Y, top.Y, bottom.Y), top.X, bottom.X);
                if (midSectionX < mid.X)
                {
                    float temp = midSectionX;
                    midSectionX = mid.X;
                    mid.X = temp;
                }
                HorizontalEdgeTriangle(top, mid.X, midSectionX, mid.Y);
                HorizontalEdgeTriangle(bottom, mid.X, midSectionX, mid.Y, skipBottomEdge: true);
            }

            _verticesPtr += 3;
        }

        private static int Highest(Vector2 v0, Vector2 v1, Vector2 v2)
        {
            return
                v0.Y > v1.Y ?
                    v0.Y > v2.Y ?
                        0 : 2
                : v1.Y > v2.Y ?
                    1 : 2;
        }

        private static void HorizontalEdgeTriangle(Vector2 apex, float leftBottomX, float rightBottomX, float bottomY, bool skipBottomEdge = false)
        {
            int dir = apex.Y > bottomY ? 1 : -1;
            float ySub = dir > 0 ? apex.Y - bottomY : bottomY - apex.Y;
            float leftSlope = (apex.X - leftBottomX) / ySub;
            float rightSlope = (apex.X - rightBottomX) / ySub;

            int endY = (int)apex.Y + dir, endX;
            Vector2Int result = new Vector2Int(0, (int)bottomY);

            if (skipBottomEdge)
                result.Y += dir;
            for (; result.Y != endY; result.Y += dir)
            {
                result.X = (int)leftBottomX;
                endX = (int)rightBottomX;
                for (; result.X <= endX; result.X++)
                    OutputRasterization(result);

                leftBottomX += leftSlope;
                rightBottomX += rightSlope;
            }
        }
    }
}

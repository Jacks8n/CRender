using System;
using CUtility.Collection;
using CUtility.Math;

using static CUtility.Math.JMathGeom;

namespace CRender.Pipeline
{
    public sealed unsafe partial class Rasterizer
    {
        private static readonly Interpolator TriangleInterpolator_Horizontal = new Interpolator();

        private static readonly Interpolator TriangleInterpolator_Vertical = new Interpolator(TriangleInterpolator_Horizontal.InterpolatedValues);

        public static void Triangle(Vector2* verticesPtr, float** verticesDataPtr, int verticesDataCount)
        {
            int topIndex =
                verticesPtr->Y > verticesPtr[1].Y ?
                    verticesPtr->Y > verticesPtr[2].Y ?
                        0 : 2
                : verticesPtr[1].Y > verticesPtr[2].Y ?
                    1 : 2, midIndex;
            switch (topIndex)
            {
                case 0:
                    midIndex = LexicoCompareDownRight(verticesPtr[1], verticesPtr[2]) < 0 ? 1 : 2;
                    break;
                case 1:
                    midIndex = LexicoCompareDownRight(verticesPtr[0], verticesPtr[2]) + 1;
                    break;
                case 2:
                    midIndex = LexicoCompareDownRight(verticesPtr[0], verticesPtr[1]) < 0 ? 0 : 1;
                    break;
                default:
                    midIndex = 1;
                    break;
            }

            Vector2 top = JMath.Floor(verticesPtr[topIndex] * _resolution);
            Vector2 mid = JMath.Floor(verticesPtr[midIndex] * _resolution);
            Vector2 bottom = JMath.Floor(verticesPtr[3 - topIndex - midIndex] * _resolution);
            bool ifInterpolate = verticesDataCount > 0;
            if (ifInterpolate)
            {
                Matrix2x2 midLineSpace;
                *(Vector2*)&midLineSpace = bottom - (top + mid) * .5f;
                *(Vector2*)&midLineSpace = top - (bottom + mid) * .5f;
                Matrix2x2.Inverse(&midLineSpace, &midLineSpace);
                TriangleInterpolator_Horizontal.SetInterpolationCount(verticesDataCount);
                TriangleInterpolator_Vertical.SetInterpolationCount(verticesDataCount);
                float* topDataPtr = verticesDataPtr[topIndex], midDataPtr = verticesDataPtr[midIndex], bottomDataPtr = verticesDataPtr[3 - topIndex - midIndex];
                float temp0, temp1;
                for (int i = 0; i < verticesDataCount; i++)
                {
                    TriangleInterpolator_Horizontal.InterpolatedValues[i] = topDataPtr[i];

                    temp0 = bottomDataPtr[i] - (topDataPtr[i] + midDataPtr[i]) * .5f;
                    temp1 = topDataPtr[i] - (bottomDataPtr[i] + midDataPtr[i]) * .5f;
                    TriangleInterpolator_Horizontal.StepValues[i] = midLineSpace.M11 * temp0 + midLineSpace.M12 * temp1;
                    TriangleInterpolator_Vertical.StepValues[i] = midLineSpace.M21 * temp0 + midLineSpace.M22 * temp1;
                }
            }

            if (JMath.Approx(top.Y, mid.Y))
                if (top.X < mid.X)
                    HorizontalEdgeTriangle(bottom, top.X, mid.X, top.Y, interpolate: ifInterpolate);
                else
                    HorizontalEdgeTriangle(bottom, mid.X, top.X, top.Y, interpolate: ifInterpolate);
            else if (JMath.Approx(mid.Y, bottom.Y))
                if (mid.X < bottom.X)
                    HorizontalEdgeTriangle(top, mid.X, bottom.X, mid.Y, interpolate: ifInterpolate);
                else
                    HorizontalEdgeTriangle(top, bottom.X, mid.X, mid.Y, interpolate: ifInterpolate);
            else
            {
                float midSectionX = JMath.Lerp(JMath.Ratio(mid.Y, top.Y, bottom.Y), top.X, bottom.X);
                if (midSectionX < mid.X)
                {
                    float temp = midSectionX;
                    midSectionX = mid.X;
                    mid.X = temp;
                }
                HorizontalEdgeTriangle(top, mid.X, midSectionX, mid.Y, interpolate: ifInterpolate);
                HorizontalEdgeTriangle(bottom, mid.X, midSectionX, mid.Y, skipBottomEdge: true, interpolate: ifInterpolate);
            }
        }

        private static void HorizontalEdgeTriangle(Vector2 apex, float leftBottomX, float rightBottomX, float bottomY, bool skipBottomEdge = false, bool interpolate = true)
        {
            if (skipBottomEdge && MathF.Abs(apex.Y - bottomY) < 1f)
                return;

            bool isUpward = apex.Y > bottomY;
            float invHeight = 1f / (isUpward ? apex.Y - bottomY : apex.Y - bottomY);
            float leftInvSlope = (apex.X - leftBottomX) * invHeight;
            float rightInvSlope = (apex.X - rightBottomX) * invHeight;

            Vector2Int result = Vector2Int.Zero;
            int endX, endY;
            if (isUpward)
            {
                endY = skipBottomEdge ? (int)bottomY + 1 : (int)bottomY;
                result.Y = (int)apex.Y;
                leftBottomX = rightBottomX = apex.X;
            }
            else
            {
                endY = (int)apex.Y;
                if (skipBottomEdge)
                {
                    result.Y = (int)bottomY - 1;
                    leftBottomX -= leftInvSlope;
                    rightBottomX -= rightInvSlope;
                }
                else
                    result.Y = (int)bottomY;
            }

            for (; result.Y >= endY; result.Y--)
            {
                endX = (int)rightBottomX;
                for (result.X = (int)leftBottomX; result.X <= endX; result.X++)
                {
                    if (interpolate)
                    {
                        OutputRasterization(result, TriangleInterpolator_Horizontal.InterpolatedValues);
                        TriangleInterpolator_Horizontal.IncrementStep();
                    }
                    else
                        OutputRasterization(result);
                }

                leftBottomX -= leftInvSlope;
                rightBottomX -= rightInvSlope;
                if (interpolate)
                    TriangleInterpolator_Vertical.IncrementStep();
            }
        }
    }
}

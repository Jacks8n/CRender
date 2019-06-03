using System;
using CUtility.Math;

namespace CRender.Pipeline
{
    public sealed unsafe partial class Rasterizer
    {
        public static void Triangle()
        {
            //Map values from -1 to 1 on values from 0 to 1
            int ut01 = (JMathGeom.IsUpper(_pointsPtr[0], _pointsPtr[1]) + 1) >> 1;
            int ut02 = (JMathGeom.IsUpper(_pointsPtr[0], _pointsPtr[2]) + 1) >> 1;
            int ut12 = (JMathGeom.IsUpper(_pointsPtr[1], _pointsPtr[2]) + 1) >> 1;

            //Ascend
            int* sortedIndices = stackalloc int[3];
            sortedIndices[ut01 + ut02] = 0;
            sortedIndices[1 - ut01 + ut12] = 1;
            sortedIndices[2 - ut02 - ut12] = 2;

            Vector2* bottomPtr = _pointsPtr + sortedIndices[0];
            Vector2* midPtr = _pointsPtr + sortedIndices[1];
            Vector2* topPtr = _pointsPtr + sortedIndices[2];
            if (JMath.Approximate(midPtr->Y, topPtr->Y))
                Triangle_HorizontalEdge(bottomPtr, topPtr, midPtr->X - topPtr->X);
            else if (JMath.Approximate(midPtr->Y, bottomPtr->Y))
                Triangle_HorizontalEdge(topPtr, midPtr, bottomPtr->X - midPtr->X);
            else
            {
                float midSplitX = JMath.Lerp(JMath.Ratio(midPtr->Y, bottomPtr->X, topPtr->X), bottomPtr->Y, topPtr->Y);
                Triangle_HorizontalEdge(topPtr, midPtr, midSplitX - midPtr->X);
                Triangle_HorizontalEdge(bottomPtr, midPtr, midSplitX - midPtr->X);
            }
        }

        /// <summary>
        /// Rasterize a triangle one of whose edges is horizontal
        /// </summary>
        /// <param name="bottomLeft">A vertex of the horizontal edge</param>
        /// <param name="width">The length of the horizontal edge, whose sign indicates the direction</param>
        private static void Triangle_HorizontalEdge(Vector2* apex, Vector2* bottomLeft, float width)
        {
            if (JMath.InRange(width, -_discardInterval.X, _discardInterval.X))
                return;

            float height = bottomLeft->Y - apex->Y;
            float scaledHeight = height * _resolution.Y;

            if (JMath.InRange(scaledHeight, -_discardInterval.Y, _discardInterval.Y))
                return;

            float scaledWidth = width * _resolution.X;
            float widthSlope = scaledWidth / scaledHeight;
            int horizontalStepDir = MathF.Sign(width);
            int verticalStepDir = MathF.Sign(scaledHeight);
            float anotherBottomX = bottomLeft->X + width;
            float apexBottomXDis = bottomLeft->X - apex->X;
            float apexAnotherXDis = anotherBottomX - apex->X;
            float horizontalStart = apex->X;
            float horizontalStartSlope = (MathF.Abs(apexBottomXDis) > MathF.Abs(apexAnotherXDis) ? apexBottomXDis : apexAnotherXDis) / MathF.Abs(height);
            int verticalEnd = JMath.RoundToInt(bottomLeft->Y * _resolution.Y);

            Vector2Int result = new Vector2Int(JMath.RoundToInt(apex->X * _resolution.X), JMath.RoundToInt(apex->Y * _resolution.Y));
            for (; result.Y != verticalEnd; result.Y += verticalStepDir)
            {
                for (float i = 0; i <= scaledWidth; i += horizontalStepDir)
                {
                    OutputRasterization(result);
                    result.X++;
                }
                scaledWidth += widthSlope;
                horizontalStart += horizontalStartSlope;
                result.X = JMath.RoundToInt(horizontalStart);
            }
        }

    }
}

using CRender.Math;

namespace CRender.Pipeline.Structure
{
    public struct TriangleFragment
    {
        /// <summary>
        /// Ascend in Y direction
        /// </summary>
        public readonly int[] Indices;

        /// <summary>
        /// Ascend in Y direction
        /// </summary>
        public readonly Vector2[] ScreenCoords;

        private TriangleFragment(int[] indices, Vector2[] screenCoords)
        {
            Indices = indices;
            ScreenCoords = screenCoords;
        }

        /// <summary>
        /// Generate a <see cref="TriangleFragment"/> with ordered <see cref="Indices"/> and <see cref="ScreenCoords"/>
        /// </summary>
        public static TriangleFragment GenerateFragment(Vector2 coord0, Vector2 coord1, Vector2 coord2, int index0, int index1, int index2)
        {
            int compareSum0102 = coord0.Y.CompareTo(coord1.Y) + coord0.Y.CompareTo(coord2.Y), compare12 = coord1.Y.CompareTo(coord2.Y);

            if (compare12 == 0)
                return compareSum0102 > 0 ?
                    new TriangleFragment(new int[] { index0, index1, index2 }, new Vector2[] { coord0, coord1, coord2 })
                    : new TriangleFragment(new int[] { index1, index2, index0 }, new Vector2[] { coord1, coord2, coord0 });
            else
            {
                Vector2[] orderedCoords = new Vector2[3];
                int[] orderedIndices = new int[3];
                int coord1Index, coord2Index;
                if ((compareSum0102 & 1) == 0)
                {
                    compareSum0102 = 2 - compareSum0102 / 2;
                    coord1Index = (compareSum0102 + compare12) / 2;
                    coord2Index = (compareSum0102 - compare12) / 2;
                }
                else
                {
                    coord1Index = 1 + compare12;
                    coord2Index = 1 - compare12;
                }
                orderedCoords[coord1Index] = coord1;
                orderedCoords[coord2Index] = coord2;
                orderedIndices[coord1Index] = index1;
                orderedIndices[coord2Index] = index2;
                orderedCoords[3 - coord1Index - coord2Index] = coord0;
                return new TriangleFragment(orderedIndices, orderedCoords);
            }
        }
    }
}
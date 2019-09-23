using CUtility.Math;
using CUtility.Math.Geometry;

namespace CRender.Pipeline.Rasterization
{
    public unsafe struct LinePrimitive : IPrimitive, ISegment
    {
        public int VertexCount => 2;

        public unsafe Vector2* CoordsPtr { get; set; }

        public unsafe float** VerticesDataPtr { get; set; }

        public int VerticesDataCount { get; set; }

        public Vector2 Vertex0 { get => CoordsPtr[0]; set => CoordsPtr[0] = value; }

        public Vector2 Vertex1 { get => CoordsPtr[1]; set => CoordsPtr[1] = value; }

        public LinePrimitive(Vector2* verticesPtr, float** verticesDataPtr, int verticesDataCount)
        {
            CoordsPtr = verticesPtr;
            VerticesDataPtr = verticesDataPtr;
            VerticesDataCount = verticesDataCount;
        }
    }
}

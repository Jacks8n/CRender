using CUtility.Math;
using CUtility.Math.Geometry;

namespace CRender.Pipeline.Rasterization
{
    public unsafe struct TrianglePrimitive : IPrimitive, ITriangle
    {
        public int VertexCount => 3;

        public Vector2* CoordsPtr { get; set; }

        public float** VerticesDataPtr { get; set; }

        public int VerticesDataCount { get; set; }

        public Vector2 Vertex0 { get => CoordsPtr[0]; set => CoordsPtr[0] = value; }

        public Vector2 Vertex1 { get => CoordsPtr[1]; set => CoordsPtr[1] = value; }

        public Vector2 Vertex2 { get => CoordsPtr[2]; set => CoordsPtr[2] = value; }
    }
}

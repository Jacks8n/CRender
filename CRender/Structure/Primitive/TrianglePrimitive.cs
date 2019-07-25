namespace CRender.Structure
{
    public readonly struct TrianglePrimitive : IPrimitive
    {
        public int VertexCount => 3;

        public int[] Indices { get; }

        public TrianglePrimitive(int index0, int index1, int index2)
        {
            Indices = new int[] { index0, index1, index2 };
        }
    }
}

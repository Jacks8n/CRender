namespace CRender.Structure
{
    public struct TrianglePrimitive : IPrimitive
    {
        public int Count => 3;

        public int[] Indices { get; }

        public TrianglePrimitive(int[] indices)
        {
            Indices = indices;
        }
    }
}

namespace CRender.Structure
{
    public struct LinePrimitive : IPrimitive
    {
        public int Count => 2;

        public int[] Indices { get; }

        public LinePrimitive(int[] indices)
        {
            Indices = indices;
        }
    }
}

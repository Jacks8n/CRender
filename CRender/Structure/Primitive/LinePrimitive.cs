namespace CRender.Structure
{
    public readonly struct LinePrimitive : IPrimitive
    {
        public int Count => 2;

        public int[] Indices { get; }

        public LinePrimitive(int index0, int index1)
        {
            Indices = new int[] { index0, index1 };
        }
    }
}

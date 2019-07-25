namespace CRender.Structure
{
    public unsafe interface IPrimitive
    {
        int VertexCount { get; }

        int[] Indices { get; }
    }
}

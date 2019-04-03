namespace CShader
{
    public unsafe interface IVertexShader
    {
        void Vertex(void* input, void* output);
    }
}
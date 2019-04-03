namespace CShader
{
    public unsafe interface IGeometryShader
    {
        void Geometry(void* input, void* output);
    }
}
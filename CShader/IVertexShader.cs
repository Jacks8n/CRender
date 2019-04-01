namespace CShader
{
    public interface IVertexShader<TVIn> : IShaderStage where TVIn : unmanaged
    {
        void Vertex(TVIn input);
    }
}
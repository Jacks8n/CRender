namespace CShader
{
    public interface IGeometryShader<TGIn> : IShaderStage where TGIn : unmanaged
    {
        void Geometry(TGIn input);
    }
}
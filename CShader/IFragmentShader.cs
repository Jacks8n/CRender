namespace CShader
{
    public interface IFragmentShader<TFIn> : IShaderStage where TFIn : unmanaged
    {
        void Fragment(TFIn input);
    }
}
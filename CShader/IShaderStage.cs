using CRender.Pipeline;

namespace CShader
{
    public unsafe interface IShaderStage<T> : IShader where T : IShaderStage<T>
    {
        void Main(void* inputPtr, void* outputPtr);
    }
}

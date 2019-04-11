namespace CShader
{
    public unsafe interface IShaderStage<T> : IShader where T : IShaderStage<T>
    {
        /// <param name="_">A placeholder, it should be null</param>
        void Main(void* inputPtr, void* outputPtr, IShaderStage<T> _);
    }
}

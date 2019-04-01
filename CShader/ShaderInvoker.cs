namespace CShader
{
    /// <summary>
    /// Under construction
    /// </summary>
    public static unsafe class ShaderInvoker
    {
        public static void InvokeVertex<TVIn>(IVertexShader<TVIn> shader) where TVIn : unmanaged
        {
            TVIn* input = stackalloc TVIn[1];
            ShaderInterpreter.Interprete<IVertexShader<TVIn>>();
            shader.Vertex(*input);
        }
    }
}

using CRender.Math;

namespace CShader
{
    public static unsafe class ShaderInvoker
    {
        private static ShaderInputCollection _currentInputCollection = null;

        private static IShaderStage _currentShader;

        public static void ChangeActiveShader<T>(T shader) where T : class, IShaderStage
        {
            _currentShader = shader;
            _currentInputCollection = ShaderInterpreter.GetInterpretedShaderInput<T>();
        }

        public static void InvokeVertex<TVIn>(Vector4 vertex) where TVIn : unmanaged
        {
            if (!(_currentShader is IVertexShader<TVIn> vertexShader))
                return;

            TVIn* input = stackalloc TVIn[1];
            _currentInputCollection.VertexInputMap.SetTargetInputStruct(input);
            _currentInputCollection.VertexInputMap.SetVertex(vertex);
            vertexShader.Vertex(*input);
        }
    }
}

using CUtility;
using CUtility.Math;

namespace CShader
{
    public unsafe class ShaderInvoker<TStage> : JSingleton<ShaderInvoker<TStage>> where TStage : IShaderStage<TStage>
    {
        private TStage _currentShader;

        private ShaderInOutMap _currentInputMap = null;

        private ShaderInOutMap _currentOutputMap = null;

        public static void ChangeActiveShader<TShader>(TShader shader) where TShader : class, TStage
        {
            ShaderInOutMap[] inoutMaps = ShaderInterpreter<TStage>.GetInterpretedInOut<TShader>();
            Instance._currentInputMap = inoutMaps[0];
            Instance._currentOutputMap = inoutMaps[1];

            Instance._currentShader = shader;
        }

        public static ShaderInOutMap Invoke(Vector4 vertex)
        {
            Instance._currentInputMap.Vertex = vertex;
            Instance._currentShader.Main(Instance._currentInputMap.TargetPtr, Instance._currentOutputMap.TargetPtr, null);
            return Instance._currentOutputMap;
        }
    }
}
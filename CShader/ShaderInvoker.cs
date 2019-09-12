using System;
using CUtility;

namespace CShader
{
    public unsafe class ShaderInvoker<TStage> : JSingleton<ShaderInvoker<TStage>> where TStage : IShaderStage<TStage>
    {
        public static ShaderInOutInstance ActiveInputMap { get; private set; }

        public static ShaderInOutInstance ActiveOutputMap { get; private set; }

        private static TStage _activeShader;

        private static Type _activeShaderType;

        public static bool ChangeActiveShader<T>(T shader) where T : IShader
        {
            return ChangeActiveShader(typeof(T), shader);
        }

        public static bool ChangeActiveShader(Type shaderType, IShader shader)
        {
            if (_activeShaderType == shaderType)
                return false;

            ShaderInOutInstance[] inoutMaps = ShaderInterpreter<TStage>.GetInterpretedInOut(shaderType);
            ActiveInputMap = inoutMaps[0];
            ActiveOutputMap = inoutMaps[1];
            _activeShader = (TStage)shader;
            _activeShaderType = shaderType;
            return true;
        }

        public static void Invoke()
        {
            _activeShader.Main(ActiveInputMap.InOutBufferPtr, ActiveOutputMap.InOutBufferPtr, null);
        }
    }
}
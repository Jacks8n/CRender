using System;
using CUtility;
using CUtility.Math;

namespace CShader
{
    public unsafe class ShaderInvoker<TStage> : JSingleton<ShaderInvoker<TStage>> where TStage : IShaderStage<TStage>
    {
        private TStage _currentShader;

        private ShaderInOutMap _currentInputMap = null;

        private ShaderInOutMap _currentOutputMap = null;

        public static void ChangeActiveShader<T>(T shader) where T : IShader
        {
            ChangeActiveShader(typeof(T), shader);
        }

        public static void ChangeActiveShader(Type shaderType, IShader shader)
        {
            ShaderInOutMap[] inoutMaps = ShaderInterpreter<TStage>.GetInterpretedInOut(shaderType);
            Instance._currentInputMap = inoutMaps[0];
            Instance._currentOutputMap = inoutMaps[1];

            Instance._currentShader = (TStage)shader;
        }

        /// <summary>
        /// Invoke shader set through <see cref="ChangeActiveShader{TShader}(TShader)"/>, set unnecessary arrays to null
        /// </summary>
        /// <param name="index">Index of vertex data to use</param>
        public static ShaderInOutMap Invoke(int index, Vector4[] vertices = null)
        {
            if (vertices != null)
                *Instance._currentInputMap.VertexPtr = vertices[index];
            Instance._currentShader.Main(Instance._currentInputMap.InOutBufferPtr, Instance._currentOutputMap.InOutBufferPtr, null);
            return Instance._currentOutputMap;
        }
    }
}
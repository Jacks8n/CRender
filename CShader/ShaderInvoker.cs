using System;
using System.Runtime.CompilerServices;
using CShader.Interpret;

namespace CShader
{
    public unsafe static class ShaderInvoker<TStage, TPattern> where TStage : IShaderStage<TStage> where TPattern : unmanaged
    {
        public static SubPatternStruct<TPattern> ActiveInputMap;

        public static SubPatternStruct<TPattern> ActiveOutputMap;

        private static TStage _activeShader;

        private static Type _activeShaderType;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ChangeActiveShader<T>(T shader) where T : IShader
        {
            ChangeActiveShader(typeof(T), shader);
        }

        public static void ChangeActiveShader(Type shaderType, IShader shader)
        {
            if (_activeShaderType == shaderType)
                return;

            SubPatternStruct<TPattern>[] inout = ShaderInterpreter<TStage, TPattern>.GetInterpretedInOut(shaderType);
            ActiveInputMap = inout[0];
            ActiveOutputMap = inout[1];
            _activeShader = (TStage)shader;
            _activeShaderType = shaderType;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Invoke(byte* inputPtr)
        {
            ActiveInputMap.Read(inputPtr);
            _activeShader.Main(ActiveInputMap.InstancePtr, ActiveOutputMap.InstancePtr, null);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Invoke(TPattern* inputPtr)
        {
            ActiveInputMap.Read(inputPtr);
            _activeShader.Main(ActiveInputMap.InstancePtr, ActiveOutputMap.InstancePtr, null);
        }
    }
}
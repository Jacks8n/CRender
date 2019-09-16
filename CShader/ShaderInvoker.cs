using System;
using System.Runtime.CompilerServices;

namespace CShader
{
    public unsafe static class ShaderInvoker<TStage> where TStage : IShaderStage<TStage>
    {
        public static MappedLayout* InputLayoutPtr { get; private set; }

        public static MappedLayout* OutputLayoutPtr { get; private set; }

        private static ShaderInOutInstance ActiveInputMap;

        private static ShaderInOutInstance ActiveOutputMap;

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

            ShaderInOutInstance[] inoutMaps = ShaderInterpreter<TStage>.GetInterpretedInOut(shaderType);
            ActiveInputMap = inoutMaps[0];
            ActiveOutputMap = inoutMaps[1];
            InputLayoutPtr = ActiveInputMap.MappedLayout;
            OutputLayoutPtr = ActiveOutputMap.MappedLayout;
            _activeShader = (TStage)shader;
            _activeShaderType = shaderType;
        }

        public static void Invoke<T>(T* inputPointer) where T : unmanaged
        {
            ActiveInputMap.Layout.MapToValues(inputPointer);
            _activeShader.Main(ActiveInputMap.InOutBufferPtr, ActiveOutputMap.InOutBufferPtr, null);
        }

        public static void Invoke(SemanticLayout input)
        {
            ActiveInputMap.Layout.AssignValues(input);
            _activeShader.Main(ActiveInputMap.InOutBufferPtr, ActiveOutputMap.InOutBufferPtr, null);
        }
    }
}
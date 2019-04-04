using System;
using CRender;
using CRender.Math;

using static CRender.MarshalExt;

namespace CShader
{
    public unsafe class ShaderInvoker<TStage> : JSingleton<ShaderInvoker<TStage>>, IDisposable where TStage : IShaderStage<TStage>
    {
        private TStage _currentShader;

        private void* _inputBufferPtr = null;

        private void* _outputBufferPtr = null;

        private ShaderInOutMap _currentInputMap = null;

        private ShaderInOutMap _currentOutputMap = null;

        public void ChangeActiveShader<TShader>() where TShader : class, TStage
        {
            ShaderInOutMap[] inoutMaps = ShaderInterpreter<TStage>.GetInterpretedInOut<TShader>();
            _currentInputMap = inoutMaps[0];
            _currentOutputMap = inoutMaps[1];

            AllocInOutBuffer(ref _inputBufferPtr, _currentInputMap);
            AllocInOutBuffer(ref _outputBufferPtr, _currentOutputMap);
        }

        public ShaderInOutMap Invoke(Vector4 vertex)
        {
            _currentInputMap.Vertex = vertex;
            _currentShader.Main(_inputBufferPtr, _outputBufferPtr);
            return _currentOutputMap;
        }

        private static void AllocInOutBuffer(ref void* ptr, ShaderInOutMap target)
        {
            if (ptr == null)
                ptr = Alloc<byte>(target.TotalSize);
            else
                ReAlloc(ptr, target.TotalSize);
            target.SetTargetInOutPtr(ptr);
        }

        void IDisposable.Dispose()
        {
            Free(_inputBufferPtr);
        }
    }
}
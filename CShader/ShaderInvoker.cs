using System;
using CRender;
using CRender.Math;

using static CRender.MarshalExt;

namespace CShader
{
    public unsafe class ShaderInvoker : JSingleton<ShaderInvoker>, IDisposable
    {
        private static ShaderInputCollection _currentInputCollection = null;

        private static IVertexShader _currentVertexShader = null;

        private static IGeometryShader _currentGeometryShader = null;

        private static IFragmentShader _currentFragmentShader = null;

        private static void* _vertexInputBufferPtr = null, _geometryInputBufferPtr = null, _fragmentInputBufferPtr = null;

        public static void ChangeActiveShader<T>(T shader) where T : class, IShaderStage
        {
            _currentInputCollection = ShaderInterpreter.GetInterpretedShaderInput<T>();

            if (_currentInputCollection.VertexInputMap != null)
            {
                _currentVertexShader = shader as IVertexShader;
                AllocInOutBuffer(ref _vertexInputBufferPtr, _currentInputCollection.VertexInputMap);
            }
            else
                _currentVertexShader = null;

            if (_currentInputCollection.GeometryInputMap != null)
            {
                _currentGeometryShader = shader as IGeometryShader;
                AllocInOutBuffer(ref _geometryInputBufferPtr, _currentInputCollection.GeometryInputMap);
            }
            else
                _currentGeometryShader = null;

            if (_currentInputCollection.FragmentInputMap != null)
            {
                _currentFragmentShader = shader as IFragmentShader;
                AllocInOutBuffer(ref _fragmentInputBufferPtr, _currentInputCollection.FragmentInputMap);
            }
            else
                _currentFragmentShader = null;
        }

        public static ShaderInOutMap InvokeVertex(Vector4 vertex)
        {
            if (_currentVertexShader == null)
                return null;
            //TODO geometry stage is optional, so it shouldn't be written as fixed
            _currentInputCollection.VertexInputMap.Vertex = vertex;
            _currentVertexShader.Vertex(_vertexInputBufferPtr, _geometryInputBufferPtr);
            return _currentInputCollection.GeometryInputMap;
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
            Free(_vertexInputBufferPtr);
            Free(_geometryInputBufferPtr);
            Free(_fragmentInputBufferPtr);
        }
    }
}
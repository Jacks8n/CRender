using System;
using System.Runtime.CompilerServices;
using CUtility.Math;

using static CUtility.Extension.MarshalExt;

namespace CShader
{
    [Flags]
    internal enum ShaderInOutSemantic { None = 0, Vertex = 1, Normal = 2 }

    public unsafe class ShaderInOutMap : IDisposable
    {
        public Vector4* VertexPtr { get; private set; } = null;

        public Vector3* NormalPtr { get; private set; } = null;

        internal byte* InOutBufferPtr { get; private set; } = null;

        private bool _generated = false;

        private ShaderInOutSemantic _registeredSemantics = ShaderInOutSemantic.None;

        private int _totalBufferSize = 0;

        public void Assign(ShaderInOutMap another)
        {
            if (VertexPtr != null && another.VertexPtr != null)
                *VertexPtr = *another.VertexPtr;
            if (NormalPtr != null && another.NormalPtr != null)
                *NormalPtr = *another.NormalPtr;
        }

        internal void RegisterSemantic(ShaderInOutSemantic semantic)
        {
            if (_generated || (_registeredSemantics & semantic) != 0)
                throw new Exception();

            _registeredSemantics |= semantic;
            _totalBufferSize += GetSemanticSize(semantic);
        }

        internal void GenerateMap()
        {
            _generated = true;
            InOutBufferPtr = AllocBytes<byte>(_totalBufferSize);
            TryRegisterSemantic(ShaderInOutSemantic.Normal,
                TryRegisterSemantic(ShaderInOutSemantic.Vertex, InOutBufferPtr));
        }

        private int GetSemanticSize(ShaderInOutSemantic semantic)
        {
            switch (semantic)
            {
                case ShaderInOutSemantic.Vertex:
                    return sizeof(Vector4);
                case ShaderInOutSemantic.Normal:
                    return sizeof(Vector3);
            }
            return 0;
        }

        private byte* TryRegisterSemantic(ShaderInOutSemantic semantic, byte* currentPtr)
        {
            if ((_registeredSemantics & semantic) != 0)
                switch (semantic)
                {
                    case ShaderInOutSemantic.Vertex:
                        VertexPtr = (Vector4*)currentPtr;
                        return currentPtr + sizeof(Vector4);
                    case ShaderInOutSemantic.Normal:
                        NormalPtr = (Vector3*)currentPtr;
                        return currentPtr + sizeof(Vector3);
                }
            return currentPtr;
        }

        void IDisposable.Dispose()
        {
            if (!_generated)
                return;

            Free(InOutBufferPtr);
        }
    }
}
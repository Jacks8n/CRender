using System;
using CUtility.Math;

using static CUtility.Extension.MarshalExt;

namespace CShader
{
    [Flags]
    internal enum ShaderInOutSemantic { None = 0, Vertex = 1, Normal = 2, UV = 4 }

    public unsafe class ShaderInOutMap : IDisposable
    {
        public bool RequireInterpolation => NormalPtr != null || UVPtr != null;

        public Vector4* VertexPtr { get; private set; } = null;

        public Vector3* NormalPtr { get; private set; } = null;

        public Vector2* UVPtr { get; private set; } = null;

        internal byte* InOutBufferPtr { get; private set; } = null;

        private bool _generated = false;

        private ShaderInOutSemantic _registeredSemantics = ShaderInOutSemantic.None;

        private int _totalBufferSize = 0;

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
                case ShaderInOutSemantic.UV:
                    return sizeof(Vector2);
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
                        break;
                    case ShaderInOutSemantic.Normal:
                        NormalPtr = (Vector3*)currentPtr;
                        break;
                    case ShaderInOutSemantic.UV:
                        UVPtr = (Vector2*)currentPtr;
                        break;
                }
            return currentPtr + GetSemanticSize(semantic);
        }

        void IDisposable.Dispose()
        {
            if (!_generated)
                return;

            Free(InOutBufferPtr);
        }
    }
}
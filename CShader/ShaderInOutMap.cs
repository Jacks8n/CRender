using System;
using CUtility.Math;

using static CUtility.Extension.MarshalExt;

namespace CShader
{
    public unsafe class ShaderInOutMap : IDisposable
    {
        public byte* InOutBufferPtr { get; private set; } = null;

        public Vector4* VertexPtr { get; private set; } = null;

        private int _vertexPtrOffset = 0;

        public void AllocInOutBuffer(int size)
        {
            if (InOutBufferPtr == null)
                InOutBufferPtr = Alloc<byte>(size);
            else
                InOutBufferPtr = ReAlloc(InOutBufferPtr, size);

            SetVertexOffset(_vertexPtrOffset);
        }

        public void SetVertexOffset(int offset)
        {
            _vertexPtrOffset = offset;
            if (InOutBufferPtr != null)
                VertexPtr = (Vector4*)(InOutBufferPtr + _vertexPtrOffset);
        }

        void IDisposable.Dispose()
        {
            if (InOutBufferPtr != null)
                Free(InOutBufferPtr);
        }
    }
}
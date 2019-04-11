using System;
using CUtility.Math;
using static CUtility.Extension.MarshalExt;

namespace CShader
{
    public unsafe class ShaderInOutMap : IDisposable
    {
        public Vector4 Vertex
        {
            get => TargetPtr != null ? *(Vector4*)(TargetPtr + VertexPtrOffset) : Vector4.Zero;
            set
            {
                if (TargetPtr != null)
                    *(Vector4*)(VertexPtrOffset + TargetPtr) = value;
            }
        }

        public int VertexPtrOffset { private get; set; } = 0;

        public byte* TargetPtr { get; private set; } = null;

        public void AllocInOutBuffer(int size)
        {
            if (TargetPtr == null)
                TargetPtr = Alloc<byte>(size);
            else
                ReAlloc(TargetPtr, size);
        }

        void IDisposable.Dispose()
        {
            if (TargetPtr != null)
                Free(TargetPtr);
        }
    }
}

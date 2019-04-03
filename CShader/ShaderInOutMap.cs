using CRender.Math;

namespace CShader
{
    public unsafe class ShaderInOutMap
    {
        public Vector4 Vertex
        {
            get => _targetPtr != null ? *((Vector4*)_targetPtr + VertexPtrOffset) : Vector4.Zero;
            set
            {
                if (_targetPtr != null)
                    *((Vector4*)_targetPtr + VertexPtrOffset) = value;
            }
        }

        public int TotalSize;

        public int VertexPtrOffset { private get; set; } = -1;

        private void* _targetPtr = null;

        public void SetTargetInOutPtr(void* targetPtr)
        {
            _targetPtr = targetPtr;
        }
    }
}

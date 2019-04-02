using CRender.Math;

namespace CShader
{
    public unsafe class ShaderInputMap
    {
        public int VertexPtrOffset { private get; set; }

        private void* _targetPtr = null;

        public void SetTargetInputStruct(void* targetPtr)
        {
            _targetPtr = targetPtr;
        }

        public void SetVertex(Vector4 vertex)
        {
            if (_targetPtr != null)
                *((Vector4*)_targetPtr + VertexPtrOffset) = vertex;
        }
    }
}

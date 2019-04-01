using CRender.Math;

namespace CShader
{
    public unsafe class ShaderInputMap
    {
        public int VertexPtrOffset { private get; set; }

        private void* _targetPtr;

        public void SetTargetInputStruct(void* targetPtr)
        {
            _targetPtr = targetPtr;
        }

        public void SetVertex(Vector4 vertex)
        {
            *((Vector4*)_targetPtr + VertexPtrOffset) = vertex;
        }
    }
}

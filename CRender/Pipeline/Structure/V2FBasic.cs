using CRender.Math;

namespace CRender.Pipeline.Structure
{
    public struct V2FBasic : IRenderData_VOut, IRenderData_FIn<V2FBasic>
    {
        Vector2 IRenderData_FIn<V2FBasic>.UV_FIn { get => _uv; set => _uv = value; }

        Vector4 IRenderData_VOut.Vertex_VOut { get => _vertex; set => _vertex = value; }

        Vector2 IRenderData_VOut.UV_VOut { get => _uv; set => _uv = value; }

        private Vector4 _vertex;

        private Vector2 _uv;

        public V2FBasic(Vector4 vertex, Vector2 uv)
        {
            _vertex = vertex;
            _uv = uv;
        }

        public unsafe void Minus(V2FBasic* other)
        {
            _vertex -= other->_vertex;
        }

        public unsafe void Add(V2FBasic* other)
        {
            throw new System.NotImplementedException();
        }

        public void Multiply(float scale)
        {
            throw new System.NotImplementedException();
        }

        public V2FBasic GetHorizontalComponent(int leftOrRight)
        {
            throw new System.NotImplementedException();
        }
    }
}

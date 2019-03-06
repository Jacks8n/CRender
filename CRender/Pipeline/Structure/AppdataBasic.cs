using CRender.Math;
using CRender.Structure;

namespace CRender.Pipeline.Structure
{
    public struct AppdataBasic : IRenderData_App<AppdataBasic>
    {
        Vector4 IRenderData_App<AppdataBasic>.Vertex_App { get => _vertex; set => _vertex = value; }

        Vector2 IRenderData_App<AppdataBasic>.UV_App { get => _uv; set => _uv = value; }

        private Vector4 _vertex;

        private Vector2 _uv;

        public AppdataBasic(Vector4 vertex, Vector2 uv)
        {
            _vertex = vertex;
            _uv = uv;
        }

        public void AssignAppdata(ref Model model, int vertexIndex)
        {
            _vertex = new Vector4(model.Vertices[vertexIndex], 1);
            _uv = model.UVs[vertexIndex];
        }
    }
}

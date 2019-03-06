using CRender.Math;

namespace CRender.Pipeline.Structure
{
    public interface IRenderData_VOut
    {
        Vector4 Vertex_VOut { get; set; }

        Vector2 UV_VOut { get; set; }
    }
}
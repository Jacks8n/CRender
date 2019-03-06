using CRender.Math;

namespace CRender.Pipeline.Structure
{
    public interface IRenderData_GOut
    {
        Vector4 Vertex_GOut { get; set; }

        Vector2 UV_GOut { get; set; }
    }
}

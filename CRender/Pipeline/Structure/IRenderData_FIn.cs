using CRender.Math;

namespace CRender.Pipeline.Structure
{
    public interface IRenderData_FIn<T> : IInterpolatable<T> where T : unmanaged, IRenderData_FIn<T>
    {
        Vector4 Vertex_FIn { get; set; }

        Vector2 UV_FIn { get; set; }
    }
}

using CRender.Math;

namespace CRender.Pipeline.Structure
{
    public interface IRenderData_FIn<T> where T : unmanaged, IRenderData_FIn<T>
    {
        Vector2 UV_FIn { get; set; }
    }
}

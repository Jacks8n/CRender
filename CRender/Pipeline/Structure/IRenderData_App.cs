using CRender.Math;
using CRender.Structure;

namespace CRender.Pipeline.Structure
{
    public interface IRenderData_App<T> where T : unmanaged, IRenderData_App<T>
    {
        Vector4 Vertex_App { get; set; }

        Vector2 UV_App { get; set; }

        /// <summary>
        /// Fetch related vertex data from <paramref name="model"/> based on <paramref name="vertexIndex"/>
        /// </summary>
        void AssignAppdata(ref Model model, int vertexIndex);
    }
}

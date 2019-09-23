using CUtility.Math;

namespace CRender.Pipeline.Rasterization
{
    public interface IPrimitiveRasterizer<T> where T : unmanaged, IPrimitive
    {
        Rasterizer.RasterizerEntry RasterizerEntry { get; }

        Vector2 Resolution { set; }

        unsafe void Rasterize(T* primitivePtr);
    }
}

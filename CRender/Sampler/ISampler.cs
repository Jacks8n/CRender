using CRender.Math;
using CRender.Structure;

namespace CRender.Sampler
{
    /// <summary>
    /// Samples a pixel accurately
    /// </summary>
    public interface ISampler
    {
        GenericVector<T> Sample<T>(RenderBuffer<T> source, Vector2Int uv) where T : unmanaged;
    }

    /// <summary>
    /// Samples a pixel ambiguously, filters are applied normally
    /// </summary>
    public interface ISamplerFloat<T> where T : unmanaged
    {
        /// <param name="u">From 0 to 1</param>
        /// <param name="v">From 0 to 1</param>
        GenericVector<T> Sample(RenderBuffer<T> source, Vector2 uv);
    }
}
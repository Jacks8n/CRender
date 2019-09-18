using CUtility.Math;

namespace CRender.Pipeline
{
    public interface IRasterzePrimitive
    {
        unsafe void Rasterize(Vector2* verticesPtr, float** verticesDataPtr, int verticesDataCount);
    }
}

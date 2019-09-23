using CUtility.Math;

namespace CRender.IO
{
    public interface IPPMImage
    {
        int Width { get; }

        int Height { get; }

        GenericVector<float> GetColorAt(int u, int v);
    }
}

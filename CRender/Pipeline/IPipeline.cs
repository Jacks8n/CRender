using CRender.Structure;

namespace CRender.Pipeline
{
    public interface IPipeline
    {
        RenderBuffer<float> RenderTarget { get; }

        RenderBuffer<float> Draw<T>(RenderEntity[] entities, T camera) where T : ICamera;
    }
}
using CRender.Structure;

namespace CRender.Pipeline
{
    public interface IPipeline
    {
        RenderBuffer<float> RenderTarget { get; }

        RenderBuffer<float> Draw(RenderEntity[] entities, ICamera camera);
    }
}
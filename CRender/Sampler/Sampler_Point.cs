using CUtility.Math;
using CRender.Structure;

namespace CRender.Sampler
{
    public class Sampler_Point : SamplerBase, ISamplerFloat<float>
    {
        public Sampler_Point(ISamplerRepeat repeatModeX, ISamplerRepeat repeatModeY) : base(repeatModeX, repeatModeY) { }

        public GenericVector<float> Sample(RenderBuffer<float> source, Vector2 uv)
        {
            uv.X = (int)(source.Width * uv.X);
            uv.Y = (int)(source.Height * uv.Y);
            return Sample(source, uv);
        }
    }
}
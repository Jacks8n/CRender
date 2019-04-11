using CRender.Structure;
using CUtility;
using CUtility.Math;

namespace CRender.Sampler
{
    public class Sampler_Bilinear : SamplerBase, ISamplerFloat<float>
    {
        public Sampler_Bilinear(ISamplerRepeat repeatModeX, ISamplerRepeat repeatModeY) : base(repeatModeX, repeatModeY) { }

        public GenericVector<float> Sample(RenderBuffer<float> source, Vector2 uv)
        {
            int intV = (int)uv.Y;
            float lerpV = uv.Y % 1f;
            var top = SampleInterpolationRow(source, uv.X, intV);
            var bottom = SampleInterpolationRow(source, uv.X, intV + 1);
            return JMath.Lerp(lerpV, bottom, top);
        }

        private GenericVector<float> SampleInterpolationRow(RenderBuffer<float> source, float u, int v)
        {
            int intU = (int)u;
            float lerpU = u % 1f;
            return JMath.Lerp(lerpU, Sample(source, new Vector2Int(intU, v)), Sample(source, new Vector2Int(intU + 1, v)));
        }
    }
}

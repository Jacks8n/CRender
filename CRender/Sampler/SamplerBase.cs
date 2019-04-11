using CUtility.Math;
using CRender.Structure;

namespace CRender.Sampler
{
    public abstract class SamplerBase : ISampler
    {
        protected ISamplerRepeat _repeatModeX;
        protected ISamplerRepeat _repeatModeY;

        public SamplerBase(ISamplerRepeat repeatModeX, ISamplerRepeat repeatModeY)
        {
            _repeatModeX = repeatModeX;
            _repeatModeY = repeatModeY;
        }

        public virtual GenericVector<T> Sample<T>(RenderBuffer<T> source, Vector2Int uv) where T : unmanaged
        {
            return source.GetPixel(_repeatModeX.GetUV(uv.X, source.Width), _repeatModeY.GetUV(uv.Y, source.Height));
        }
    }
}
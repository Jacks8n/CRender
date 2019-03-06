using CRender.Math;

namespace CRender.Sampler
{
    public class SamplerRepeat_Clamp : JSingleton<SamplerRepeat_Clamp>, ISamplerRepeat
    {
        public int GetUV(int uORv, int widthORheight)
        {
            return System.Math.Clamp(uORv, 0, widthORheight - 1);
        }
    }
}

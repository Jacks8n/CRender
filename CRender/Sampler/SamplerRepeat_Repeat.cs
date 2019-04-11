using CUtility;

namespace CRender.Sampler
{
    public class SamplerRepeat_Repeat : JSingleton<SamplerRepeat_Repeat>, ISamplerRepeat
    {
        public int GetUV(int uORv, int widthORheight)
        {
            return uORv % widthORheight;
        }
    }
}

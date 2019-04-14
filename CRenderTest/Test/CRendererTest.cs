using CRender;
using CRender.Structure;
using CUtility.Math;

namespace CRenderTest
{
    class CRendererTest
    {
        public static void TestDrawBuffer()
        {
            RenderBuffer<float> buffer = new RenderBuffer<float>(CRenderSettings.RenderWidth, CRenderSettings.RenderHeight, channelCount: 3);
            CharRenderBuffer<float> charBuffer = new CharRenderBuffer<float>(buffer);

            GenericVector<float> white = new GenericVector<float>(3) { 1, 1, 1 };
            buffer.WritePixel(0, 0, white);
            buffer.WritePixel(8, 2, white);
            buffer.WritePixel(16, 4, white);
            buffer.WritePixel(24, 6, white);
            buffer.WritePixel(32, 8, white);
            CRenderer.Render(charBuffer);
        }

        public static void TestDrawColor()
        {
            RenderBuffer<float> buffer = new RenderBuffer<float>(CRenderSettings.RenderWidth, CRenderSettings.RenderHeight, channelCount: 3);
            CharRenderBuffer<float> charBuffer = new CharRenderBuffer<float>(buffer);

            GenericVector<float> color = new GenericVector<float>(3) { 0, 0, 0 };
            for (int i = 0; i < buffer.Width; i++)
            {
                for (int j = 0; j < buffer.Height; j++)
                    buffer.WritePixel(i, j, color);
                color.Write((float)i / buffer.Width);
            }
            CRenderer.Render(charBuffer);
        }

    }
}

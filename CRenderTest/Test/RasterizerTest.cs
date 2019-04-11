using System;
using System.Threading;
using CRender;
using CUtility.Math;
using CRender.Pipeline;
using CRender.Structure;
using NUnit.Framework;

namespace CRenderTest
{
    class RasterizerTest
    {
        public static unsafe void TestRasterize()
        {
            Rasterizer.StartRasterize(CRenderSettings.ResolutionF);
            RenderBuffer<float> tex0 = new RenderBuffer<float>(CRenderSettings.Resolution, 3);
            CharRenderBuffer<float> texChar = new CharRenderBuffer<float>(tex0);
            Vector2* points = stackalloc Vector2[4];

            for (float i = 0; i < JMath.PI_TWO; i += .02f)
            {
                Vector2 dir = new Vector2(MathF.Cos(i) * .3f, MathF.Sin(i) * .3f);
                Vector2 orthoDir = new Vector2(-dir.Y, dir.X);
                points[0] = new Vector2(.5f, .5f) + dir;
                points[1] = new Vector2(.5f, .5f) - dir;
                points[2] = new Vector2(.5f, .5f) + orthoDir;
                points[3] = new Vector2(.5f, .5f) - orthoDir;
                Rasterizer.SetPoints(points);
                Rasterizer.Line();
                Rasterizer.Line();
                tex0.WritePixel(Rasterizer.ContriveResult(), new GenericVector<float>(3) { 1f, 1f, 1f });
                CRenderer.Render(texChar);
                tex0.Clear();
                Thread.Sleep(16);
            }

            Rasterizer.EndRasterize();
        }

        [Test]
        public static void TestRasterizerStartEnd()
        {
            Rasterizer.StartRasterize(new Vector2(100, 100));
            Assert.Catch(() => Rasterizer.StartRasterize(new Vector2(100, 100)));
            Rasterizer.EndRasterize();
            Assert.Catch(() => Rasterizer.EndRasterize());
        }

        [Test]
        public static unsafe void TestRasterizerLine()
        {
            Vector2* line = stackalloc Vector2[2] { Vector2.Zero, Vector2.One };
            Rasterizer.StartRasterize(new Vector2(100, 100));
            Rasterizer.SetPoints(line);
            Rasterizer.Line();
            var resultArr = Rasterizer.ContriveResult();
            Assert.AreEqual(resultArr.Length, 100, 1);
            Rasterizer.EndRasterize();
        }
    }
}

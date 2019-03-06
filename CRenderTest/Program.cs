using CRender.Math;
using CRender;
using CRender.Pipeline;
using CRender.Structure;
using NUnit.Framework;

using static System.Console;

namespace CRenderTest
{
    [TestFixture]
    public class Program
    {
        private static void Main(string[] args)
        {
            RenderBuffer<float> buffer = new RenderBuffer<float>(WindowWidth, WindowHeight, 3);
            buffer.WritePixel(0, 0, new GenericVector<float>(3) { 1, 1, 1 });
            CRenderer.Render(buffer);
            ReadKey();
        }

        [Test]
        public static void TestRasterizerStart()
        {
            Rasterizer.StartRasterize(new Vector2(100, 100));
            Assert.Catch(() => Rasterizer.StartRasterize(new Vector2(100, 100)));
        }

        [Test]
        public static void TestRenderBufferWrite()
        {
            RenderBuffer<float> buffer = new RenderBuffer<float>(10, 10, 3);
            buffer.WritePixel(0, 0, new GenericVector<float>(3) { 3, 6, 10 });
            Assert.AreEqual(buffer[0][0].R, 3);
            Assert.AreEqual(buffer[0][0].G, 6);
            Assert.AreEqual(buffer[0][0].B, 10);
        }

        [Test]
        public static void TestGenericVectorWrite()
        {
            GenericVector<float> vector1 = new GenericVector<float>(3) { 1, 4, 7 };
            GenericVector<float> vector2 = new GenericVector<float>(3) { 4, 6, 12 };
            vector1.Write(vector2);
            Assert.AreEqual(vector1.R, 4);
            Assert.AreEqual(vector1.G, 6);
            Assert.AreEqual(vector1.B, 12);
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
        }

        [Test]
        public static void TestGenericVector()
        {
            GenericVector<int> rgba = new GenericVector<int>(4) { 2, 25, 43, 65 };
            Assert.AreEqual(rgba.R, 2);
            Assert.AreEqual(rgba.G, 25);
            Assert.AreEqual(rgba.B, 43);
            Assert.AreEqual(rgba.A, 65);
            Assert.AreEqual(rgba.RGB.R, 2);
            Assert.AreEqual(rgba.RGB.G, 25);
            Assert.AreEqual(rgba.RGB.B, 43);
            Assert.AreEqual(rgba.RGB.RG.R, 2);
            Assert.AreEqual(rgba.RGB.RG.G, 25);
        }
    }
}
using System.Threading;
using CRender;
using CRender.Math;
using CRender.Pipeline;
using CRender.Pipeline.Structure;
using CRender.Structure;
using NUnit.Framework;

using static System.Console;

namespace CRenderTest
{
    [TestFixture]
    public class Program
    {
        private static unsafe void Main(string[] args)
        {
            //Rotation seems to be the bug
            CRenderSettings.IsCountFrames = true;
            TestDrawLine();
            //TestRenderFrames();
            ReadKey();
        }

        public static void TestRenderFrames()
        {
            RenderBuffer<float> buffer = new RenderBuffer<float>(CRenderSettings.RenderWidth, CRenderSettings.RenderHeight, 3);
            CharRenderBuffer<float> charBuffer = new CharRenderBuffer<float>(buffer);

            float framerate = 25f;
            float time = 10f;
            int totalFrame = (int)(framerate * time);
            int frameInterval = (int)(1000f / framerate);
            GenericVector<float> white = new GenericVector<float>(3) { 1, 1, 1 };
            GenericVector<float> black = new GenericVector<float>(3) { 0, 0, 0 };
            int lastU = 0;
            int step = totalFrame / buffer.Width + 1;
            for (int i = 0; i < totalFrame; i++)
            {
                if (i / step > lastU)
                {
                    buffer.WritePixel(lastU, 0, black);
                    buffer.WritePixel(++lastU, 0, white);
                }
                Thread.Sleep(frameInterval);
                CRenderer.Render(charBuffer);
            }
        }

        public static void TestDrawLine()
        {
            PipelineBase<AppdataBasic, V2FBasic> pipeline = new PipelineBase<AppdataBasic, V2FBasic>();
            CharRenderBuffer<float> charBuffer = new CharRenderBuffer<float>(pipeline.RenderTarget);

            RenderEntity entity = new RenderEntity(new Transform(Vector3.Zero),
                new Model(
                    vertices: new Vector3[] { Vector3.Zero, Vector3.UnitXPositive, Vector3.UnitYPositive },//, Vector3.UnitZPositive },
                    primitives: new IPrimitive[] { new LinePrimitive(0, 1), new LinePrimitive(0, 2) },// new LinePrimitive(0, 3) },
                    uvs: null,
                    normals: null
                ), null);
            ICamera camera = new Camera_Orthographic(width: 10f, height: 10f, near: -10, far: 10,
                new Transform(
                pos: Vector3.Zero,
                rotation: new Vector3(0, JMath.PI_HALF, 0)));
            RenderEntity[] entitiesApply = new RenderEntity[] { entity };

            float framerate = 25f;
            float time = 10f;
            int totalFrame = (int)(framerate * time);
            float angleStep = JMath.PI_TWO / totalFrame;
            int frameInterval = (int)(1000f / framerate);
            for (int i = 0; i < totalFrame; i++)
            {
                pipeline.Draw(entitiesApply, camera);
                CRenderer.Render(charBuffer);
                entity.Transform.Rotation.Z += angleStep;
                Thread.Sleep(frameInterval);
            }
        }

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

        [Test]
        public static void TestTransformCopyInstance()
        {
            Transform transform = new Transform(Vector3.UnitXPositive);
            Transform copy = transform.GetInstanceToApply();
            Assert.AreNotEqual(transform, copy);
            Assert.AreEqual(copy.Position.X, transform.Position.X);
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
            Rasterizer.EndRasterize();
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

        [Test]
        public static void TestLocalToWorldMatrix_Translation()
        {
            Transform transform = new Transform(Vector3.UnitXPositive);
            Matrix4x4 l2w = transform.LocalToWorld;
            Vector4 world0 = l2w * new Vector4(Vector3.Zero, 1);
            Vector4 world1 = l2w * Vector4.UnitXPositive_Point;
            Vector4 world2 = l2w * Vector4.UnitYPositive_Point;
            Vector4 world3 = l2w * Vector4.UnitZPositive_Point;
            Assert.AreEqual(world0.X, 1);
            Assert.AreEqual(world0.Y, 0);
            Assert.AreEqual(world0.Z, 0);
            Assert.AreEqual(world1.X, 2);
            Assert.AreEqual(world1.Y, 0);
            Assert.AreEqual(world1.Z, 0);
            Assert.AreEqual(world2.X, 1);
            Assert.AreEqual(world2.Y, 1);
            Assert.AreEqual(world2.Z, 0);
            Assert.AreEqual(world3.X, 1);
            Assert.AreEqual(world3.Y, 0);
            Assert.AreEqual(world3.Z, 1);
        }

        [Test]
        public static void TestWorldToLocalMatrix_Translation()
        {
            Transform transform = new Transform(Vector3.UnitXPositive);
            Matrix4x4 l2w = transform.WorldToLocal;
            Vector4 world0 = l2w * new Vector4(Vector3.Zero, 1);
            Vector4 world1 = l2w * Vector4.UnitXPositive_Point;
            Vector4 world2 = l2w * Vector4.UnitYPositive_Point;
            Vector4 world3 = l2w * Vector4.UnitZPositive_Point;
            Assert.AreEqual(world0.X, -1);
            Assert.AreEqual(world0.Y, 0);
            Assert.AreEqual(world0.Z, 0);
            Assert.AreEqual(world1.X, 0);
            Assert.AreEqual(world1.Y, 0);
            Assert.AreEqual(world1.Z, 0);
            Assert.AreEqual(world2.X, -1);
            Assert.AreEqual(world2.Y, 1);
            Assert.AreEqual(world2.Z, 0);
            Assert.AreEqual(world3.X, -1);
            Assert.AreEqual(world3.Y, 0);
            Assert.AreEqual(world3.Z, 1);
        }

        [Test]
        public static void TestLocalToWorldMatrix_Rotation()
        {
            Transform transform = new Transform();
            transform.Rotation.X = JMath.PI_HALF;
            Matrix4x4 x_pi_half = transform.LocalToWorld;
            transform.Rotation.Y = JMath.PI_HALF;
            Matrix4x4 xy_pi_half = transform.LocalToWorld;
            transform.Rotation.Z = JMath.PI_HALF;
            Matrix4x4 xyz_pi_half = transform.LocalToWorld;
            Vector4 world0 = x_pi_half * Vector4.One;
            Vector4 world1 = xy_pi_half * Vector4.One;
            Vector4 world2 = xyz_pi_half * Vector4.One;
            Assert.AreEqual(world0.X, 1, 1e-5f);
            Assert.AreEqual(world0.Y, -1, 1e-5f);
            Assert.AreEqual(world0.Z, 1, 1e-5f);
            Assert.AreEqual(world1.X, 0, 1e-5f);
            Assert.AreEqual(world1.Y, -1, 1e-5f);
            Assert.AreEqual(world1.Z, -1, 1e-5f);
            Assert.AreEqual(world2.X, 0, 1e-5f);
            Assert.AreEqual(world2.Y, 1, 1e-5f);
            Assert.AreEqual(world2.Z, -1, 1e-5f);
        }

        [Test]
        public static void TestWorldToLocalMatrix_Rotation()
        {
            Transform transform = new Transform();
            transform.Rotation.X = JMath.PI_HALF;
            Matrix4x4 x_pi_half = transform.WorldToLocal;
            transform.Rotation.Y = JMath.PI_HALF;
            Matrix4x4 xy_pi_half = transform.WorldToLocal;
            transform.Rotation.Z = JMath.PI_HALF;
            Matrix4x4 xyz_pi_half = transform.WorldToLocal;
            Vector4 world0 = x_pi_half * Vector4.One;
            Vector4 world1 = xy_pi_half * Vector4.One;
            Vector4 world2 = xyz_pi_half * Vector4.One;
            Assert.AreEqual(world0.X, 1, 1e-5f);
            Assert.AreEqual(world0.Y, 1, 1e-5f);
            Assert.AreEqual(world0.Z, -1, 1e-5f);
            Assert.AreEqual(world1.X, -1, 1e-5f);
            Assert.AreEqual(world1.Y, 1, 1e-5f);
            Assert.AreEqual(world1.Z, -1, 1e-5f);
            Assert.AreEqual(world2.X, -1, 1e-5f);
            Assert.AreEqual(world2.Y, 1, 1e-5f);
            Assert.AreEqual(world2.Z, 1, 1e-5f);
        }
    }
}
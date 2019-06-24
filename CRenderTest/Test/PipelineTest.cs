using System.Threading;
using CRender;
using CRender.Pipeline;
using CRender.Structure;
using CShader.Sample;
using CUtility;
using CUtility.Math;

namespace CRenderTest
{
    class PipelineTest
    {
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
            EstablishTestScene(out var pipeline, out var charBuffer, out var camera);

            RenderEntity entity = new RenderEntity(
                transform: new Transform(Vector3.Zero),
                model: Model.Cube(),
                material: new Material<Shader_Distort>(Shader_Distort.Instance));
            RenderEntity[] entitiesApply = new RenderEntity[] { entity };

            DrawRotatingObject(pipeline, entitiesApply, camera, charBuffer);
        }

        public static void TestRenderTriangle()
        {
            EstablishTestScene(out var pipeline, out var charBuffer, out var camera);

            RenderEntity entity = new RenderEntity(
                transform: new Transform(Vector3.Zero),
                new Model(
                    vertices: new Vector4[] { new Vector4(-.5f, -.25f, 0, 1f), new Vector4(.5f, -.25f, 0, 1f), new Vector4(0, .5f, 0, 1f) },
                    primitives: new IPrimitive[] { new TrianglePrimitive(0, 1, 2) },
                    uvs: null,
                    normals: null),
                material: null);
            RenderEntity[] entitiesApply = new RenderEntity[] { entity };

            DrawRotatingObject(pipeline, entitiesApply, camera, charBuffer);
        }

        public static void TestRenderFaces()
        {
            EstablishTestScene(out var pipeline, out var charBuffer, out var camera);

            RenderEntity entity = new RenderEntity(
                transform: new Transform(Vector3.Zero),
                model: Model.Plane(),
                material: null);
            RenderEntity[] entitiesApply = new RenderEntity[] { entity };

            DrawRotatingObject(pipeline, entitiesApply, camera, charBuffer);
        }

        public static void TestRenderCube()
        {
            EstablishTestScene(out var pipeline, out var charBuffer, out var camera);

            RenderEntity entity = new RenderEntity(
                transform: new Transform(Vector3.Zero),
                model: Model.Cube(isWireframe: false),
                material: null);
            RenderEntity[] entitiesApply = new RenderEntity[] { entity };

            DrawRotatingObject(pipeline, entitiesApply, camera, charBuffer);
        }

        /// <param name="camera">It orients the origin</param>
        private static void EstablishTestScene(out Pipeline pipeline, out CharRenderBuffer<float> charBuffer, out ICamera camera)
        {
            pipeline = new Pipeline();
            charBuffer = new CharRenderBuffer<float>(pipeline.RenderTarget);
            camera = new Camera_Orthographic(width: 3.5f, height: 3.5f, near: -2.5f, far: 2.5f,
                new Transform(
                pos: Vector3.Zero,
                rotation: new Vector3(0, JMath.PI_HALF * .35f, -JMath.PI_HALF * 1.5f)));
        }

        private static void DrawRotatingObject(IPipeline pipeline, RenderEntity[] entitiesApply, ICamera camera, CharRenderBuffer<float> charBuffer)
        {
            float framerate = 25f;
            float time = 10f;
            int totalFrame = (int)(framerate * time);
            float angleStep = JMath.PI_TWO / totalFrame;
            int frameInterval = (int)(1000f / framerate);

            JTimer timer = new JTimer();
            timer.Start();
            for (int i = 0; i < totalFrame; i++)
            {
                CRenderer.UpdateRenderInfo();
                pipeline.Draw(entitiesApply, camera);
                entitiesApply[0].Transform.Rotation.X += angleStep;
                entitiesApply[0].Transform.Rotation.Z += angleStep;

                int elapsed = (int)timer.DeltaMS;
                if (elapsed < frameInterval)
                    Thread.Sleep(frameInterval - elapsed);
                CRenderer.Render(charBuffer);
            }
            timer.Stop();
        }
    }
}

using System.Threading;
using CRender;
using CRender.Pipeline;
using CRender.Structure;
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
            EstablishTestScene<Camera_Orthographic>(out var pipeline, out var charBuffer, out var camera);

            RenderEntity entity = new RenderEntity(
                transform: new Transform(Vector3.Zero),
                model: Model.Cube(),
                material: null);
            RenderEntity[] entitiesApply = new RenderEntity[] { entity };

            DrawRotatingObject(pipeline, entitiesApply, camera, charBuffer);
        }

        public static void TestRenderTriangle()
        {
            EstablishTestScene<Camera_Orthographic>(out var pipeline, out var charBuffer, out var camera);

            RenderEntity entity = new RenderEntity(
                transform: new Transform(Vector3.Zero),
                new Model(
                    vertices: new Vector3[] { new Vector3(-.5f, -.25f, 0), new Vector3(.5f, -.25f, 0), new Vector3(0, .5f, 0) },
                    indices: new int[] { 0, 1, 2 },
                    uvs: null,
                    normals: null),
                material: null);
            RenderEntity[] entitiesApply = new RenderEntity[] { entity };

            DrawRotatingObject(pipeline, entitiesApply, camera, charBuffer);
        }

        public static void TestRenderFaces()
        {
            EstablishTestScene<Camera_Orthographic>(out var pipeline, out var charBuffer, out var camera);

            RenderEntity entity = new RenderEntity(
                transform: new Transform(Vector3.Zero),
                model: Model.Plane(),
                material: null);
            RenderEntity[] entitiesApply = new RenderEntity[] { entity };

            DrawRotatingObject(pipeline, entitiesApply, camera, charBuffer);
        }

        public static void TestRenderCube()
        {
            EstablishTestScene<Camera_Orthographic>(out var pipeline, out var charBuffer, out var camera);

            RenderEntity entity = new RenderEntity(
                transform: new Transform(Vector3.Zero),
                model: Model.Cube(),
                material: null);
            RenderEntity[] entitiesApply = new RenderEntity[] { entity };

            DrawRotatingObject(pipeline, entitiesApply, camera, charBuffer);
        }

        /// <param name="camera">It orients the origin</param>
        public static void EstablishTestScene<T>(out Pipeline pipeline, out CharRenderBuffer<float> charBuffer, out ICamera camera) where T : ICamera
        {
            pipeline = new Pipeline();
            charBuffer = new CharRenderBuffer<float>(pipeline.RenderTarget);
            camera = new Camera_Orthographic(width: 3.5f, height: 3.5f, near: -2.5f, far: 2.5f,
                new Transform(
                pos: Vector3.Zero,
                rotation: new Vector3(0, JMath.PI_HALF * .35f, -JMath.PI_HALF * 1.5f)));
        }

        public static void DrawRotatingObject<TPipeline, TCamera>(TPipeline pipeline, RenderEntity[] entitiesApply, TCamera camera, CharRenderBuffer<float> charBuffer) where TPipeline : IPipeline where TCamera : ICamera
        {
            float framerate = 60f;
            float time = 6f;
            int totalFrame = (int)(framerate * time);
            float angleStep = JMath.PI_TWO / totalFrame;
            int frameInterval = (int)(1000f / framerate);
            int elapsed;

            JTimer timer = new JTimer();
            timer.Start();
            for (int i = 0; i < totalFrame; i++)
            {
                CRenderer.UpdateRenderInfo();
                pipeline.Draw(entitiesApply, camera);
                entitiesApply[0].Transform.Rotation.X += angleStep;
                entitiesApply[0].Transform.Rotation.Z += angleStep;

                elapsed = (int)timer.DeltaMS;
                if (elapsed < frameInterval)
                    Thread.Sleep(frameInterval - elapsed);
                CRenderer.Render(charBuffer);
            }
            timer.Stop();
        }

        public static void DrawOneFrame<TPipeline, TCamera>(TPipeline pipeline, RenderEntity[] entitiesApply, TCamera camera, CharRenderBuffer<float> charBuffer) where TPipeline : IPipeline where TCamera : ICamera
        {
            CRenderer.UpdateRenderInfo();
            pipeline.Draw(entitiesApply, camera);
            CRenderer.Render(charBuffer);
        }
    }
}

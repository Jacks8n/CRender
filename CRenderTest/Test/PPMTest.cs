using System;
using CRender.IO;
using CRender.Structure;
using CShader;

namespace CRenderTest
{
    public static class PPMTest
    {
        public static void TestSaveImageToDesktop()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            path += "\\test.ppm";
            PipelineTest.EstablishTestScene<Camera_Orthographic>(out var pipeline, out var charBuffer, out var camera);
            PipelineTest.DrawOneFrame(pipeline,
                new RenderEntity[] { new RenderEntity(new Transform(), Model.Plane(1f), Material.NewMaterial(ShaderDefault.Instance)) },
                camera, charBuffer);
            pipeline.RenderTarget.SaveAsPPM(path);
        }
    }
}

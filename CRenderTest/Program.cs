using CRender;
using NUnit.Framework;

using static System.Console;

namespace CRenderTest
{
    [TestFixture]
    public class Program
    {
        private static void Main(string[] args)
        {
            WriteLine("Press any key to run test");
            ReadKey();
            CRenderSettings.SetFontSize(10);
            CRenderSettings.IsCountFrames = true;
            CRenderSettings.IsShowFPS = true;

            PipelineTest.TestRenderFaces();
            //PipelineTest.TestRenderCube();
            //PipelineTest.TestRenderTriangle();
            //PipelineTest.TestDrawLine();
            //PipelineTest.TestRenderFrames();
            //RasterizerTest.TestRasterize();

            ReadKey();
        }
    }
}
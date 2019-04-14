using CRender;
using NUnit.Framework;

using static System.Console;

namespace CRenderTest
{
    [TestFixture]
    public class Program
    {
        private static unsafe void Main(string[] args)
        {
            WindowHeight = 50;
            CRenderSettings.IsCountFrames = true;
            WriteLine("Press any key to run test");
            ReadKey();

            //PipelineTest.TestRenderTriangle();
            PipelineTest.TestDrawLine();
            //PipelineTest.TestRenderFrames();

            ReadKey();
        }
    }
}
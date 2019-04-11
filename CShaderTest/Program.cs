using CShader;
using CUtility.Math;
using NUnit.Framework;

using static System.Console;

namespace CShaderTest
{
    [TestFixture]
    class Program
    {
        static void Main(string[] args)
        {
            ReadKey();
        }

        [Test]
        public void TestShaderInterpret()
        {
            ShaderInterpreter<IVertexShader>.InterpretAll();
            ShaderInterpreter<IFragmentShader>.InterpretAll();
        }

        [Test]
        public void TestShaderInvoke()
        {
            TestShaderInterpret();
            var shader = new TestShader();
            ShaderInvoker<IVertexShader>.ChangeActiveShader(shader);
            ShaderInvoker<IFragmentShader>.ChangeActiveShader(shader);

            Vector4 pos = new Vector4(-1f, 2f, -3f, 4f);
            Vector4 vertexResult = ShaderInvoker<IVertexShader>.Invoke(pos).Vertex;
            Vector4 fragmentResult = ShaderInvoker<IFragmentShader>.Invoke(pos).Vertex;

            Assert.AreEqual(vertexResult, new Vector4(-2.5f, 5f, -7.5f, 10f));
            Assert.AreEqual(fragmentResult, new Vector4(0f, 3f, -2f, 5f));
        }
    }
}
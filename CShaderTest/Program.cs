using System.Diagnostics;
using CShader;
using CUtility.Math;
using NUnit.Framework;
using static System.Console;

namespace CShaderTest
{
    [TestFixture]
    class Program
    {
        static unsafe void Main(string[] args)
        {
            var shader = new TestShader();
            ShaderInterpreter<IVertexShader>.Interpret<TestShader>();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            ShaderInvoker<IVertexShader>.ChangeActiveShader(shader);
            *ShaderInvoker<IVertexShader>.ActiveInputMap.VertexPtr = Vector4.Zero;
            for (int i = 0; i < 480000; i++)
                ShaderInvoker<IVertexShader>.Invoke();
            sw.Stop();
            WriteLine(sw.ElapsedMilliseconds);
            ReadKey();
        }

        [Test]
        public void TestShaderInterpret()
        {
            ShaderInterpreter<IVertexShader>.InterpretAll();
            ShaderInterpreter<IFragmentShader>.InterpretAll();
        }

        [Test]
        public unsafe void TestShaderInvoke()
        {
            TestShaderInterpret();
            TestShader shader = new TestShader();
            ShaderInvoker<IVertexShader>.ChangeActiveShader(shader);
            ShaderInvoker<IFragmentShader>.ChangeActiveShader(shader);

            *ShaderInvoker<IVertexShader>.ActiveInputMap.VertexPtr
                = *ShaderInvoker<IFragmentShader>.ActiveInputMap.VertexPtr
                = new Vector4(-1f, 2f, -3f, 4f);
            ShaderInvoker<IVertexShader>.Invoke();
            Vector4 vertexResult = *ShaderInvoker<IVertexShader>.ActiveOutputMap.VertexPtr;
            Vector4 fragmentResult = *ShaderInvoker<IFragmentShader>.ActiveOutputMap.VertexPtr;

            Assert.AreEqual(vertexResult, new Vector4(-2.5f, 5f, -7.5f, 10f));
            Assert.AreEqual(fragmentResult, new Vector4(0f, 3f, -2f, 5f));
        }

        [Test]
        public unsafe void TestVertexShaderTransform()
        {
            var shader = ShaderDefault.Instance;
            ShaderInvoker<IVertexShader>.ChangeActiveShader(shader);
            Matrix4x4.Translation(Vector3.One, ShaderValue.ObjectToView);

            Vector4[] pos = new Vector4[] { Vector4.UnitXPositive_Point, Vector4.UnitYPositive_Point, Vector4.UnitZPositive_Point };
            for (int i = 0; i < pos.Length; i++)
            {
                *ShaderInvoker<IVertexShader>.ActiveInputMap.VertexPtr = pos[i];
                ShaderInvoker<IVertexShader>.Invoke();
                Assert.AreEqual(*ShaderInvoker<IVertexShader>.ActiveOutputMap.VertexPtr, new Vector4(pos[i].XYZ + Vector3.One, 1));
            }
        }
    }
}
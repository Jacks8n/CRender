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
        const int itr_count = 16;

        static unsafe void Main(string[] args)
        {
            var shader = new TestShader();
            ShaderInterpreter<IVertexShader>.Interpret<TestShader>();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            ShaderInvoker<IVertexShader>.ChangeActiveShader(shader);
            Vector4[] input = new Vector4[itr_count];
            for (int i = 0; i < 480000 / itr_count; i++)
            {
                for (int j = 0; j < itr_count; j++)
                {
                    input[j].X = i;
                    input[j].Y = i;
                    input[j].Z = i;
                    input[j].W = i;
                }
                ShaderInvoker<IVertexShader>.Invoke(itr_count, input);
            }
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
            var shader = new TestShader();
            ShaderInvoker<IVertexShader>.ChangeActiveShader(shader);
            ShaderInvoker<IFragmentShader>.ChangeActiveShader(shader);

            Vector4[] pos = new Vector4[] { new Vector4(-1f, 2f, -3f, 4f) };
            Vector4 vertexResult = *ShaderInvoker<IVertexShader>.Invoke(0, pos).VertexPtr;
            Vector4 fragmentResult = *ShaderInvoker<IFragmentShader>.Invoke(0, pos).VertexPtr;

            Assert.AreEqual(vertexResult, new Vector4(-2.5f, 5f, -7.5f, 10f));
            Assert.AreEqual(fragmentResult, new Vector4(0f, 3f, -2f, 5f));
        }

        [Test]
        public unsafe void TestVertexShaderTransform()
        {
            var shader = ShaderDefault.Instance;
            ShaderInvoker<IVertexShader>.ChangeActiveShader(shader);
            ShaderValue.ObjectToView = Matrix4x4.Translation(Vector3.One);

            Vector4[] pos = new Vector4[] { Vector4.UnitXPositive_Point, Vector4.UnitYPositive_Point, Vector4.UnitZPositive_Point };
            for (int i = 0; i < pos.Length; i++)
            {
                var output = ShaderInvoker<IVertexShader>.Invoke(i, pos);
                Assert.AreEqual(*output.VertexPtr, new Vector4(pos[i].XYZ + Vector3.One, 1));
            }
        }
    }
}
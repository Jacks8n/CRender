using CShader;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using CUtility.Math;

namespace CShaderTest
{
    public struct VertexInputTest
    {
        public Vector4 Position;
    }

    public struct FragmentInputTest
    {
        public Vector4 Position;
    }

    public struct FragmentOutputTest
    {
        public Vector4 Position;
    }

    public class TestShader : IVertexShader, IFragmentShader
    {
        /// <summary>
        /// Outputs 2.5 times the <paramref name="inputPtr"/>'s pointing value
        /// </summary>
        public unsafe void Main(
            [ShaderInput(typeof(VertexInputTest))] void* inputPtr,
            [ShaderOutput(typeof(FragmentInputTest))] void* outputPtr, IShaderStage<IVertexShader> _)
        {
            VertexInputTest* vin = (VertexInputTest*)inputPtr;
            FragmentInputTest* vout = (FragmentInputTest*)outputPtr;

            vout->Position = vin->Position * 2.5f;
        }

        /// <summary>
        /// Outputs <paramref name="inputPtr"/>'s pointing value with every channel added 1
        /// </summary>
        public unsafe void Main(
            [ShaderInput(typeof(FragmentInputTest))] void* inputPtr,
            [ShaderOutput(typeof(FragmentOutputTest))] void* outputPtr, IShaderStage<IFragmentShader> _)
        {
            FragmentInputTest* fin = (FragmentInputTest*)inputPtr;
            FragmentOutputTest* fout = (FragmentOutputTest*)outputPtr;

            fout->Position = fin->Position + 1;
        }
    }
}

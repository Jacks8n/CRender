using CRender.Math;

namespace CShader
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

    public class ShaderTest : IVertexShader, IFragmentShader
    {
        unsafe void IShaderStage<IVertexShader>.Main(
            [ShaderInput(typeof(VertexInputTest))] void* inputPtr,
            [ShaderOutput(typeof(FragmentInputTest))] void* outputPtr)
        {
            VertexInputTest* vin = (VertexInputTest*)inputPtr;
            FragmentInputTest* vout = (FragmentInputTest*)outputPtr;

            vout->Position = vin->Position * 2;
        }

        unsafe void IShaderStage<IFragmentShader>.Main(
            [ShaderInput(typeof(FragmentInputTest))] void* inputPtr,
            [ShaderOutput(typeof(FragmentOutputTest))] void* outputPtr)
        {
            FragmentInputTest* fin = (FragmentInputTest*)inputPtr;
            FragmentOutputTest* fout = (FragmentOutputTest*)outputPtr;

            fout->Position = fin->Position + 1;
        }
    }
}

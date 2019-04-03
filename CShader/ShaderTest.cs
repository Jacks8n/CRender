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

    public class ShaderTest : IVertexShader
    {
        public unsafe void Vertex(
            [ShaderInput(typeof(VertexInputTest))] void* input,
            [ShaderOutput(typeof(FragmentInputTest))]void* output)
        {
            VertexInputTest* vin = (VertexInputTest*)input;
            FragmentInputTest* vout = (FragmentInputTest*)output;

            vout->Position = vin->Position * 2;
        }
    }
}

using CUtility;
using CUtility.Math;

using static CShader.ShaderValue;
using static CUtility.Math.Matrix4x4;

using AppData = CShader.ShaderInOutDefault.App_Base;
using VOutData = CShader.ShaderInOutDefault.VOut_Base;

namespace CShader.Sample
{
    /// <summary>
    /// Only supports VertexStage
    /// </summary>
    public unsafe class Shader_Scale : JSingleton<Shader_Scale>, IVertexShader
    {
        static Shader_Scale()
        {
            ShaderInterpreter<IVertexShader>.Interpret<Shader_Scale>();
        }

        public void Main(
            [ShaderInput(typeof(AppData))] void* inputPtr,
            [ShaderOutput(typeof(VOutData))] void* outputPtr, IShaderStage<IVertexShader> _)
        {
            AppData* appPtr = (AppData*)inputPtr;
            VOutData* vOutPtr = (VOutData*)outputPtr;

            Vector4 vertex = appPtr->Vertex * (SinTime * .3f + 1);
            Mul(ObjectToView, &vertex, &vOutPtr->Vertex);
        }
    }
}

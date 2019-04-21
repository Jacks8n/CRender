using CUtility;

using static CShader.ShaderValue;

using AppData = CShader.ShaderInOutDefault.AppData_Base;
using VOutdata = CShader.ShaderInOutDefault.VOutData_Base;

namespace CShader
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
            [ShaderOutput(typeof(VOutdata))] void* outputPtr, IShaderStage<IVertexShader> _)
        {
            AppData* appPtr = (AppData*)inputPtr;
            VOutdata* vOutPtr = (VOutdata*)outputPtr;

            vOutPtr->Vertex = ObjectToView * appPtr->Vertex * (SinTime * .3f + 1);
        }
    }
}

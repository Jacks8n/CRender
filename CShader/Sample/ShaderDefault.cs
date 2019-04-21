using CUtility;

using AppData = CShader.ShaderInOutDefault.AppData_Base;
using VOutdata = CShader.ShaderInOutDefault.VOutData_Base;

using static CShader.ShaderValue;

namespace CShader
{
    /// <summary>
    /// Only supports VertexStage
    /// </summary>
    public unsafe class ShaderDefault : JSingleton<ShaderDefault>, IVertexShader
    {
        static ShaderDefault()
        {
            ShaderInterpreter<IVertexShader>.Interpret<ShaderDefault>();
        }

        public void Main(
            [ShaderInput(typeof(AppData))] void* inputPtr,
            [ShaderOutput(typeof(VOutdata))] void* outputPtr, IShaderStage<IVertexShader> _)
        {
            AppData* appPtr = (AppData*)inputPtr;
            VOutdata* vOutPtr = (VOutdata*)outputPtr;

            vOutPtr->Vertex = ObjectToView * appPtr->Vertex;
        }
    }
}

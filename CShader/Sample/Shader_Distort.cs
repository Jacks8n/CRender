using System;
using CUtility;
using CUtility.Math;

using AppData = CShader.ShaderInOutDefault.AppData_Base;
using VOutData = CShader.ShaderInOutDefault.VOutData_Base;

using static CShader.ShaderValue;

namespace CShader.Sample
{
    public class Shader_Distort : JSingleton<Shader_Distort>, IVertexShader
    {
        static Shader_Distort()
        {
            ShaderInterpreter<IVertexShader>.Interpret<Shader_Distort>();
        }

        public unsafe void Main(
            [ShaderInput(typeof(AppData))] void* inputPtr,
            [ShaderOutput(typeof(VOutData))] void* outputPtr, IShaderStage<IVertexShader> _)
        {
            AppData* appPtr = (AppData*)inputPtr;
            VOutData* vOutPtr = (VOutData*)outputPtr;

            Vector4 pos = appPtr->Vertex;
            pos.Z *= MathF.Sin(pos.X + pos.Y + Time * .5f);
            vOutPtr->Vertex = ObjectToView * pos;
        }
    }
}

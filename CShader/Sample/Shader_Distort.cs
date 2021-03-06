﻿using System;
using CShader.Attribute;
using CShader.Interpret;
using CUtility;
using CUtility.Math;

using static CShader.ShaderValue;
using static CUtility.Math.Matrix4x4;

using AppData = CShader.ShaderInOutDefault.App_Base;
using VOutData = CShader.ShaderInOutDefault.VOut_Base;

namespace CShader.Sample
{
    public class Shader_Distort : JSingleton<Shader_Distort>, IVertexShader
    {
        static Shader_Distort()
        {
            ShaderInterpreter<IVertexShader, ShaderInOutPatternDefault>.Interpret<Shader_Distort>();
        }

        public unsafe void Main(
            [ShaderInput(typeof(AppData))] void* inputPtr,
            [ShaderOutput(typeof(VOutData))] void* outputPtr, IShaderStage<IVertexShader> _)
        {
            AppData* appPtr = (AppData*)inputPtr;
            VOutData* vOutPtr = (VOutData*)outputPtr;

            Vector4 pos = appPtr->Vertex;
            pos.Z *= MathF.Sin(pos.X + pos.Y + Time * .5f);
            Mul(ObjectToScreen, &pos, &vOutPtr->Vertex);
        }
    }
}

using System;
using CShader;

namespace CRender.Structure
{
    public interface IMaterial
    {
        Type ShaderType { get; }

        IShader Shader { get; }
    }
}
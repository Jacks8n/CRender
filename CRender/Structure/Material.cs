using CShader;
using System;
using CRender.Pipeline;

namespace CRender.Structure
{
    public class Material<T> : IMaterial where T : class, IShader
    {
        public Type ShaderType { get; }

        public IShader Shader { get; }

        public Material(T shader)
        {
            ShaderType = typeof(T);
            Shader = shader;
        }
    }
}
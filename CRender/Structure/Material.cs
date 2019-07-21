using System;
using CRender.Pipeline;
using CShader;

namespace CRender.Structure
{
    public class Material
    {
        public Type ShaderType { get; private set; }

        public IShader Shader { get; private set; }

        public void SetShader<T>(T shader) where T : IShader
        {
            ShaderType = typeof(T);
            Shader = shader;
        }

        public static Material NewMaterial<T>(T shader) where T : IShader
        {
            Material material = new Material();
            material.SetShader(shader);
            return material;
        }
    }
}
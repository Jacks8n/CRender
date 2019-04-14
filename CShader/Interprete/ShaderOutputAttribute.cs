using System;

namespace CShader
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public class ShaderOutputAttribute : ShaderInOutAttributeBase
    {
        public ShaderOutputAttribute(Type outputType) : base(outputType)
        {
        }
    }
}
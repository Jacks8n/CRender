using System;

namespace CShader
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public class ShaderInputAttribute : ShaderInOutAttributeBase
    {
        public ShaderInputAttribute(Type inputType) : base(inputType)
        {
        }
    }
}
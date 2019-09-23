using System;

namespace CShader.Attribute
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public class ShaderInputAttribute : TypeBasedAttributeBase
    {
        public ShaderInputAttribute(Type inputType) : base(inputType)
        {
        }
    }
}
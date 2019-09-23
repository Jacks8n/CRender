using System;

namespace CShader.Attribute
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public class ShaderOutputAttribute : TypeBasedAttributeBase
    {
        public ShaderOutputAttribute(Type outputType) : base(outputType)
        {
        }
    }
}
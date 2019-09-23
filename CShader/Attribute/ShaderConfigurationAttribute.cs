using System;

namespace CShader.Attribute
{
    public class ShaderConfigurationAttribute : TypeBasedAttributeBase
    {
        public ShaderConfigurationAttribute(Type type) : base(type)
        {
        }
    }
}

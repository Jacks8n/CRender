using System;

namespace CShader
{
    public abstract class ShaderInOutAttributeBase : Attribute
    {
        public readonly Type Type;

        public ShaderInOutAttributeBase(Type type)
        {
            Type = type;
        }
    }
}

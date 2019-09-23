using System;

namespace CShader.Attribute
{
    public abstract class TypeBasedAttributeBase : System.Attribute
    {
        public readonly Type Type;

        protected TypeBasedAttributeBase(Type type)
        {
            Type = type;
        }
    }
}

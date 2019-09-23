using System;
using System.Reflection;
using CShader.Attribute;
using System.Collections.Generic;
using System.Text;

namespace CShader.Interpret
{
    public static class ShaderConfigurationInterpreter<TConfiguration> where TConfiguration : unmanaged
    {
        public static void Interpret(Type shaderType)
        {
            Type configurationType = shaderType.GetCustomAttribute<ShaderConfigurationAttribute>()?.Type;
            if (configurationType == null)
                return;
        }
    }
}

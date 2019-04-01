using System;
using System.Collections.Generic;
using System.Reflection;

namespace CShader
{
    public static class ShaderInterpreter
    {
        private class ShaderStageInput
        {
            public readonly ShaderInputMap VertexInputMap;

            public readonly ShaderInputMap GeometryInputMap;

            public readonly ShaderInputMap FragmentInputMap;

            public ShaderStageInput(ShaderInputMap vertexInputMap, ShaderInputMap geometryInputMap, ShaderInputMap fragmentInputMap)
            {
                VertexInputMap = vertexInputMap;
                GeometryInputMap = geometryInputMap;
                FragmentInputMap = fragmentInputMap;
            }
        }

        private const string NAME_VERTEX = "Vertex";
        private const string NAME_GEOMETRY = "Geometry";
        private const string NAME_FRAGMENT = "Fragment";

        private static readonly Dictionary<Type, ShaderStageInput> InterpretedShaderInputs = new Dictionary<Type, ShaderStageInput>();

        public static void Interprete<T>() where T : class, IShaderStage
        {
            Type type = typeof(T);
            if (InterpretedShaderInputs.ContainsKey(type))
                return;

            InterpretedShaderInputs.Add(type, new ShaderStageInput(
                InterpreteMethod<T>(NAME_VERTEX),
                InterpreteMethod<T>(NAME_GEOMETRY),
                InterpreteMethod<T>(NAME_FRAGMENT)));
        }

        private static ShaderInputMap InterpreteMethod<T>(string method) where T : class, IShaderStage
        {
            MethodInfo vertexInfo = typeof(T).GetMethod(method);
            if (vertexInfo != null)
            {
                ParameterInfo[] parameters = vertexInfo.GetParameters();
                if (parameters.Length == 1)
                    return ShaderInputInterpreter.Interprete(parameters[0].ParameterType);
            }
            return null;
        }
    }
}

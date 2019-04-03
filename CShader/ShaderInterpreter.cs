using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CShader
{
    public static class ShaderInterpreter
    {
        private const string NAME_VERTEX = "Vertex";
        private const string NAME_GEOMETRY = "Geometry";
        private const string NAME_FRAGMENT = "Fragment";

        private static readonly Dictionary<Type, ShaderInputCollection> InterpretedShaderInputs = new Dictionary<Type, ShaderInputCollection>();

        private static bool _initialized = false;

        /// <summary>
        /// Interpret all the shader found through reflection
        /// </summary>
        public static void InterpretAll()
        {
            if (_initialized)
                return;

            foreach (Type type in Assembly
                .GetExecutingAssembly()
                .DefinedTypes
                .Where(item =>
                    item.IsClass
                    && item.ImplementedInterfaces.Contains(typeof(IShaderStage))))
                Interpret(type);
            _initialized = true;
        }

        public static ShaderInputCollection GetInterpretedShaderInput<T>() where T : class, IShaderStage
        {
            if (!_initialized)
                throw new Exception("Invoke InterpretAll() at first to interpret shader");

            InterpretedShaderInputs.TryGetValue(typeof(T), out ShaderInputCollection inputCollection);
            return inputCollection;
        }

        private static ShaderInputCollection Interpret(Type type)
        {
            if (InterpretedShaderInputs.TryGetValue(type, out ShaderInputCollection inputCollection))
                return inputCollection;

            inputCollection = new ShaderInputCollection(
                InterpretMethod(type, NAME_VERTEX),
                InterpretMethod(type, NAME_GEOMETRY),
                InterpretMethod(type, NAME_FRAGMENT));
            InterpretedShaderInputs.Add(type, inputCollection);
            return inputCollection;
        }

        private static ShaderInOutMap InterpretMethod(Type type, string method)
        {
            MethodInfo methodInfo = type.GetMethod(method);
            if (methodInfo != null)
            {
                ParameterInfo[] parameters = methodInfo.GetParameters();

                if (parameters.Length == 2)
                {
                    ShaderInOutInterpreter.InterpretOutput(parameters[1]);
                    return ShaderInOutInterpreter.InterpretInput(parameters[0]);
                }
                throw new Exception($"{method} must have only one parameter");
            }
            return null;
        }
    }
}

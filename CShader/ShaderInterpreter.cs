using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CShader
{
    public static class ShaderInterpreter<TStage> where TStage : IShaderStage<TStage>
    {
        private static readonly Dictionary<Type, ShaderInOutMap[]> InterpretedShaderInputs = new Dictionary<Type, ShaderInOutMap[]>();

        private static readonly Type[] _paramTypes = new Type[] { typeof(void*), typeof(void*), typeof(IShaderStage<TStage>) };

        private static bool _initialized = false;

        static ShaderInterpreter()
        {
            if (!typeof(TStage).IsInterface)
                throw new Exception("TStage must be an interface representing a shader stage");
        }

        /// <summary>
        /// Interpret all the shader found through reflection
        /// </summary>
        public static void InterpretAll()
        {
            if (_initialized)
                return;

            foreach (Type type in Assembly
                .GetCallingAssembly()
                .DefinedTypes
                .Where(item =>
                    item.IsClass
                    && item.ImplementedInterfaces.Contains(typeof(TStage))))
                InterpretMethod(type);
            _initialized = true;
        }

        public static ShaderInOutMap[] GetInterpretedInOut<TShader>() where TShader : class, TStage
        {
            InterpretedShaderInputs.TryGetValue(typeof(TShader), out ShaderInOutMap[] inoutMap);
            return inoutMap;
        }

        private static void InterpretMethod(Type shaderType)
        {
            MethodInfo methodInfo = shaderType.GetMethod("Main", _paramTypes);
            if (methodInfo == null)
                return;

            ParameterInfo[] parameters = methodInfo.GetParameters();
            InterpretedShaderInputs.Add(shaderType, new ShaderInOutMap[] {
                ShaderInOutInterpreter.InterpretInput(parameters[0]),
                ShaderInOutInterpreter.InterpretOutput(parameters[1]) });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CShader
{
    public static class ShaderInterpreter<TStage> where TStage : IShaderStage<TStage>
    {
        private const string METHOD_NAME_MAIN = "Main";

        private static readonly Type[] METHOD_ARG_TYPES = new Type[] { typeof(void*), typeof(void*), typeof(IShaderStage<TStage>) };

        private static readonly Dictionary<Type, ShaderInOutInstance[]> InterpretedShaderInputs = new Dictionary<Type, ShaderInOutInstance[]>();

        static ShaderInterpreter()
        {
            if (!typeof(TStage).IsInterface)
                throw new Exception("TStage must be an interface representing a shader stage");
        }

        /// <summary>
        /// Interpret all the shader found in the calling assembly through reflection
        /// </summary>
        public static void InterpretAll()
        {
            InterpretAll(Assembly.GetCallingAssembly());
        }

        /// <summary>
        /// Interpret all the shader found through reflection
        /// </summary>
        public static void InterpretAll(Assembly targetAssembly)
        {
            foreach (Type type in targetAssembly
                .DefinedTypes
                .Where(item =>
                    item.IsClass
                    && item.ImplementedInterfaces.Contains(typeof(TStage))))
                InterpretMethod(type);
        }

        public static ShaderInOutInstance[] GetInterpretedInOut(Type type)
        {
            if (InterpretedShaderInputs.TryGetValue(type, out ShaderInOutInstance[] inoutMap))
                return inoutMap;
            throw new Exception($"{type} hasn't been interpreted");
        }

        public static void Interpret<T>() where T : class, TStage
        {
            InterpretMethod(typeof(T));
        }

        private static void InterpretMethod(Type shaderType)
        {
            if (InterpretedShaderInputs.ContainsKey(shaderType))
                return;

            MethodInfo mainMethodInfo = shaderType.GetMethod(METHOD_NAME_MAIN, METHOD_ARG_TYPES);
            if (mainMethodInfo == null)
                return;

            ParameterInfo[] parameters = mainMethodInfo.GetParameters();
            InterpretedShaderInputs.Add(shaderType, new ShaderInOutInstance[] {
                ShaderInOutInterpreter.InterpretInput(parameters[0]),
                ShaderInOutInterpreter.InterpretOutput(parameters[1]) });
        }
    }
}

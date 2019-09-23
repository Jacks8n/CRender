using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CShader.Attribute;

namespace CShader.Interpret
{
    public static class ShaderInterpreter<TStage, TPattern> where TStage : IShaderStage<TStage> where TPattern : unmanaged
    {
        private const string METHOD_NAME_MAIN = "Main";

        private static readonly Type[] METHOD_ARG_TYPES = new Type[] { typeof(void*), typeof(void*), typeof(IShaderStage<TStage>) };

        private static readonly Dictionary<Type, SubPatternStruct<TPattern>[]> InterpretedInOutOffsets = new Dictionary<Type, SubPatternStruct<TPattern>[]>();

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
                Interpret(type);
        }

        public static SubPatternStruct<TPattern>[] GetInterpretedInOut(Type type)
        {
            if (InterpretedInOutOffsets.TryGetValue(type, out SubPatternStruct<TPattern>[] subStruct))
                return subStruct;
            throw new Exception($"{type} hasn't been interpreted");
        }

        public static void Interpret<T>() where T : class, TStage
        {
            Interpret(typeof(T));
        }

        private static void Interpret(Type type)
        {
            InterpretMethod(type);
        }

        private static void InterpretMethod(Type shaderType)
        {
            if (InterpretedInOutOffsets.ContainsKey(shaderType))
                return;

            MethodInfo mainMethodInfo = shaderType.GetMethod(METHOD_NAME_MAIN, METHOD_ARG_TYPES);
            if (mainMethodInfo == null)
                return;

            ParameterInfo[] parameters = mainMethodInfo.GetParameters();
            InterpretedInOutOffsets.Add(shaderType, new SubPatternStruct<TPattern>[] {
                InterpretInOut<ShaderInputAttribute>(parameters[0]),
                InterpretInOut<ShaderOutputAttribute>(parameters[1]) });
        }

        private static SubPatternStruct<TPattern> InterpretInOut<T>(ParameterInfo inoutType) where T : TypeBasedAttributeBase
        {
            Type type = inoutType.GetCustomAttribute<T>()?.Type;
            if (type != null)
                return StructInterpreter<TPattern>.Interpret(type);
            throw new Exception("Shader Input/Output Attribute is required for parameters");
        }
    }
}

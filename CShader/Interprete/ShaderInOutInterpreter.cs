using System;
using System.Collections.Generic;
using System.Reflection;

namespace CShader
{
    public static class ShaderInOutInterpreter
    {
        private static readonly Dictionary<Type, ShaderInOutInstance> InterpretedInOutMap = new Dictionary<Type, ShaderInOutInstance>();

        public static ShaderInOutInstance GetInterpretedMap<TInOut>()
        {
            if (InterpretedInOutMap.TryGetValue(typeof(TInOut), out ShaderInOutInstance inoutMap))
                return inoutMap;
            else
                throw new Exception($"{typeof(TInOut).ToString()} hasn't been interpreted");
        }

        public static ShaderInOutInstance InterpretInput(ParameterInfo inputType)
        {
            return Interpret<ShaderInputAttribute>(inputType);
        }

        public static ShaderInOutInstance InterpretOutput(ParameterInfo outputType)
        {
            return Interpret<ShaderOutputAttribute>(outputType);
        }

        private static ShaderInOutInstance Interpret<T>(ParameterInfo inoutType) where T : ShaderInOutAttributeBase
        {
            Type type = inoutType.GetCustomAttribute<T>()?.Type;

            if (type == null)
                throw new Exception("Shader Input/Output Attribute is required for parameter and return value");
            if (InterpretedInOutMap.TryGetValue(type, out ShaderInOutInstance inoutMap))
                return inoutMap;

            FieldInfo[] inoutFields = type.GetFields();
            SemanticLayout layout = new SemanticLayout();
            layout.BeginRegister();
            for (int i = 0; i < inoutFields.Length; i++)
                layout.RegisterSemantic(inoutFields[i].Name);
            layout.EndRegister();

            inoutMap = new ShaderInOutInstance(layout);
            inoutMap.Instantiate();
            InterpretedInOutMap.Add(type, inoutMap);
            return inoutMap;
        }
    }
}

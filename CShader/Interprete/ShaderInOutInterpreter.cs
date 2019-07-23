using System;
using System.Collections.Generic;
using System.Reflection;

namespace CShader
{
    public static class ShaderInOutInterpreter
    {
        private const string NAME_VERTEX = "Vertex";
        private const string NAME_NORMAL = "Normal";

        private static readonly Dictionary<Type, ShaderInOutMap> InterpretedInOutMap = new Dictionary<Type, ShaderInOutMap>();

        public static ShaderInOutMap GetInterpretedMap<TInOut>()
        {
            InterpretedInOutMap.TryGetValue(typeof(TInOut), out ShaderInOutMap inoutMap);
            return inoutMap;
        }

        public static ShaderInOutMap InterpretInput(ParameterInfo inputType)
        {
            return Interpret<ShaderInputAttribute>(inputType);
        }

        public static ShaderInOutMap InterpretOutput(ParameterInfo outputType)
        {
            return Interpret<ShaderOutputAttribute>(outputType);
        }

        private static ShaderInOutMap Interpret<T>(ParameterInfo inoutType) where T : ShaderInOutAttributeBase
        {
            Type type = inoutType.GetCustomAttribute<T>()?.Type;

            if (type == null)
                throw new Exception("Shader Input/Output Attribute is required for parameter and return value");
            if (InterpretedInOutMap.TryGetValue(type, out ShaderInOutMap inoutMap))
                return inoutMap;

            FieldInfo[] fields = type.GetFields();
            inoutMap = new ShaderInOutMap();

            for (int i = 0; i < fields.Length; i++)
                switch (fields[i].Name)
                {
                    case NAME_VERTEX:
                        inoutMap.RegisterSemantic(ShaderInOutSemantic.Vertex);
                        break;
                    case NAME_NORMAL:
                        inoutMap.RegisterSemantic(ShaderInOutSemantic.Normal);
                        break;
                }
            inoutMap.GenerateMap();

            InterpretedInOutMap.Add(type, inoutMap);
            return inoutMap;
        }
    }
}

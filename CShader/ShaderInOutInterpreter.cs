using System;
using System.Collections.Generic;
using System.Reflection;

namespace CShader
{
    public static class ShaderInOutInterpreter
    {
        private const string NAME_MEMBER_VERTEX = "Vertex";

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

            int ptrOffset = 0;
            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo field = fields[i];
                switch (field.Name)
                {
                    case NAME_MEMBER_VERTEX:
                        inoutMap.SetVertexOffset(ptrOffset);
                        break;
                }
                ptrOffset += SizeOfHelper.SizeOf(field.FieldType);
            }
            inoutMap.AllocInOutBuffer(ptrOffset);

            InterpretedInOutMap.Add(type, inoutMap);
            return inoutMap;
        }
    }
}

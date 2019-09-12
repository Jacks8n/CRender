using System;
using System.Collections.Generic;
using System.Reflection;

namespace CShader
{
    public static class ShaderInOutInterpreter
    {
        private const string SEMATIC_VERTEX = "Vertex";
        private const string SEMATIC_NORMAL = "Normal";
        private const string SEMATIC_UV = "UV";
        private const string SEMATIC_COLOR = "Color";

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
                switch (inoutFields[i].Name)
                {
                    case SEMATIC_VERTEX:
                        layout.RegisterVertex();
                        break;
                    case SEMATIC_NORMAL:
                        layout.RegisterNormal();
                        break;
                    case SEMATIC_UV:
                        layout.RegisterUV();
                        break;
                    case SEMATIC_COLOR:
                        layout.RegisterColor();
                        break;
                }
            layout.EndRegister();

            inoutMap = new ShaderInOutInstance(layout);
            inoutMap.Instantiate();
            InterpretedInOutMap.Add(type, inoutMap);
            return inoutMap;
        }
    }
}

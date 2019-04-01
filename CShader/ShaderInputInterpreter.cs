using System;
using System.Collections.Generic;
using System.Reflection;

namespace CShader
{
    public static class ShaderInputInterpreter
    {
        private const string NAME_VERTEX = "Vertex";

        private static readonly Dictionary<Type, ShaderInputMap> InterpretedInputMap = new Dictionary<Type, ShaderInputMap>();

        public static ShaderInputMap Interprete(Type type)
        {
            if (InterpretedInputMap.TryGetValue(type, out ShaderInputMap value))
                return value;

            MemberInfo[] members = type.GetMembers();
            ShaderInputMap inputMap = new ShaderInputMap();

            int ptrOffset = 0;
            for (int i = 0; i < members.Length; i++)
                switch (members[i].Name)
                {
                    case NAME_VERTEX:
                        inputMap.VertexPtrOffset = ptrOffset;
                        ptrOffset += SizeOfHelper.SizeOf(members[i].ReflectedType);
                        break;
                }

            InterpretedInputMap.Add(type, inputMap);
            return inputMap;
        }
    }
}

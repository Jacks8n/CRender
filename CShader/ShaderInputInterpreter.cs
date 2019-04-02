using System;
using System.Collections.Generic;
using System.Reflection;

namespace CShader
{
    public static class ShaderInputInterpreter
    {
        private const string NAME_VERTEX = "Vertex";

        private static readonly Dictionary<Type, ShaderInputMap> InterpretedInputMap = new Dictionary<Type, ShaderInputMap>();

        public static ShaderInputMap Interpret(Type type)
        {
            if (InterpretedInputMap.TryGetValue(type, out ShaderInputMap inputMap))
                return inputMap;

            MemberInfo[] members = type.GetMembers();
            inputMap = new ShaderInputMap();

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

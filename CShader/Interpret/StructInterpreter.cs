using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace CShader.Interpret
{
    internal unsafe static class StructInterpreter<TPattern> where TPattern : unmanaged
    {
        internal static readonly int[] PatternFieldOffsets;

        internal static readonly int[] PatternFieldSizes;

        internal static int PatternFieldCount => _patternFields.Length;

        private static readonly Dictionary<Type, SubPatternStruct<TPattern>> _interpretedStruct = new Dictionary<Type, SubPatternStruct<TPattern>>();

        private static readonly FieldInfo[] _patternFields;

        static StructInterpreter()
        {
            Type type = typeof(TPattern);
            if (!type.IsValueType)
                throw new Exception($"{type.FullName} isn't a struct");

            _patternFields = type.GetFields();
            PatternFieldOffsets = new int[_patternFields.Length];
            PatternFieldSizes = new int[_patternFields.Length];
            int totalSize = 0;
            for (int i = 0; i < PatternFieldOffsets.Length; i++)
            {
                PatternFieldOffsets[i] = totalSize;
                totalSize += PatternFieldSizes[i] = SizeOfHelper.SizeOf(_patternFields[i].FieldType);
            }
        }

        internal static SubPatternStruct<TPattern> Interpret(Type structType)
        {
            if (_interpretedStruct.TryGetValue(structType, out SubPatternStruct<TPattern> interpretedStruct))
                return interpretedStruct;
            if (!IsSequentialLayout(structType))
                throw new Exception("Struct may be sequential struct layout");

            FieldInfo[] fields = structType.GetFields();
            int[] mappedOffset = new int[_patternFields.Length];
            int* fieldOffsets = stackalloc int[fields.Length];
            int totalSize = 0;
            for (int i = 0; i < fields.Length; i++)
            {
                fieldOffsets[i] = totalSize;
                totalSize += SizeOfHelper.SizeOf(fields[i].FieldType);
            }

            int index;
            for (int i = 0; i < _patternFields.Length; i++)
            {
                index = Array.FindIndex(fields, (item) => IsEqualFieldInfo(item, _patternFields[i]));
                mappedOffset[i] = index >= 0 ? fieldOffsets[index] : -1;
            }

            interpretedStruct = new SubPatternStruct<TPattern>(mappedOffset, totalSize);
            _interpretedStruct.Add(structType, interpretedStruct);
            return interpretedStruct;
        }

        internal static SubPatternStruct<TPattern> GetInterpretedStruct(Type structType)
        {
            return _interpretedStruct[structType];
        }

        private static bool IsSequentialLayout(Type type) => type.StructLayoutAttribute?.Value == LayoutKind.Sequential;

        private static bool IsEqualFieldInfo(FieldInfo l, FieldInfo r) => l.Name == r.Name && l.FieldType == r.FieldType;
    }
}

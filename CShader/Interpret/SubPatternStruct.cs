using System;
using CShader.Interpret;
using System.Runtime.CompilerServices;

using static CUtility.Extension.MarshalExtension;

namespace CShader
{
    public unsafe class SubPatternStruct<TPattern> : IDisposable where TPattern : unmanaged
    {
        public readonly byte* InstancePtr;

        public readonly int InstanceSize;

        public readonly int SizeInFloatCount;

        private readonly int[] _structFieldOffsets = null;

        internal SubPatternStruct(int[] fieldOffsets, int totalSize)
        {
            InstancePtr = Alloc<byte>(totalSize);
            InstanceSize = totalSize;
            SizeInFloatCount = totalSize / sizeof(float);
            _structFieldOffsets = fieldOffsets;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Read(byte* sourcePtr)
        {
            Move(sourcePtr, InstancePtr, InstanceSize);
        }

        /// <summary>
        /// <paramref name="destinationPtr"/> may have same layout which <see cref="SubPatternStruct{TPattern}"/> represents
        /// </summary>
        public void MoveFields(TPattern* sourcePtr, byte* destinationPtr)
        {
            byte* srcPtr = (byte*)sourcePtr;
            for (int i = 0; i < _structFieldOffsets.Length; i++)
                if (_structFieldOffsets[i] >= 0)
                    MoveField(srcPtr + StructInterpreter<TPattern>.PatternFieldOffsets[i], destinationPtr + _structFieldOffsets[i], i);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Read(TPattern* sourcePtr) => MoveFields(sourcePtr, InstancePtr);

        public void Write(SubPatternStruct<TPattern> destination)
        {
            byte* srcPtr = InstancePtr, destPtr = destination.InstancePtr;
            for (int i = 0; i < StructInterpreter<TPattern>.PatternFieldCount; i++)
                if (_structFieldOffsets[i] > 0 && destination._structFieldOffsets[i] > 0)
                    MoveField(srcPtr + _structFieldOffsets[i], destPtr + destination._structFieldOffsets[i], i);
        }

        public void Write(TPattern* destinationPtr)
        {
            byte* srcPtr = InstancePtr, destPtr = (byte*)destinationPtr;
            for (int i = 0; i < _structFieldOffsets.Length; i++)
                if (_structFieldOffsets[i] >= 0)
                    MoveField(srcPtr + _structFieldOffsets[i], destPtr + StructInterpreter<TPattern>.PatternFieldOffsets[i], i);
        }

        /// <summary>
        /// <paramref name="srcPtr"/> and <paramref name="destPtr"/> may be sub structs of the same <typeparamref name="TPattern"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void MoveField(byte* srcPtr, byte* destPtr, int fieldIndex)
        {
            Move(srcPtr, destPtr, StructInterpreter<TPattern>.PatternFieldSizes[fieldIndex]);
        }

        void IDisposable.Dispose()
        {
            Free(InstancePtr);
        }
    }
}

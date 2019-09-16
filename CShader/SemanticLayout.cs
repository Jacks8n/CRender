using System;
using CUtility.Math;

using static CUtility.Extension.MarshalExtension;

namespace CShader
{
    [Flags]
    internal enum ShaderSemantic { None = 0, Vertex = 1, Normal = 2, UV = 4, Color = 8, All = Vertex | Normal | UV | Color }

    public unsafe struct MappedLayout
    {
        public Vector4* VertexPtr { get; internal set; }

        public Vector3* NormalPtr { get; internal set; }

        public Vector2* UVPtr { get; internal set; }

        public Vector4* ColorPtr { get; internal set; }
    }

    public unsafe partial class SemanticLayout : IDisposable
    {
        internal int TotalBufferSize { get; private set; }

        internal ShaderSemantic RegisteredSemantic { get; private set; }

        internal readonly MappedLayout* MappedLayoutPtr;

        private const int OFFSET_NULL = -1;

        private bool _isRegistering = false;

        private int _vertexOffset = OFFSET_NULL;

        private int _normalOffset = OFFSET_NULL;

        private int _uvOffset = OFFSET_NULL;

        private int _colorOffset = OFFSET_NULL;

        public SemanticLayout()
        {
            MappedLayoutPtr = Alloc<MappedLayout>();
        }

        void IDisposable.Dispose()
        {
            Free(MappedLayoutPtr);
        }
    }
}
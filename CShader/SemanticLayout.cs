using System;
using CUtility.Math;

using static CUtility.Extension.MarshalExtension;

namespace CShader
{
    [Flags]
    internal enum ShaderSemantic { None = 0, Vertex = 1, Normal = 2, UV = 4, Color = 8, All = Vertex | Normal | UV | Color }

    public unsafe class SemanticLayout : IDisposable
    {
        public struct MappedLayout
        {
            public Vector4* VertexPtr { get; internal set; }

            public Vector3* NormalPtr { get; internal set; }

            public Vector2* UVPtr { get; internal set; }

            public Vector4* ColorPtr { get; internal set; }
        }

        internal int TotalBufferSize { get; private set; }

        private readonly MappedLayout* MappedLayoutPtr;

        public SemanticLayout()
        {
            MappedLayoutPtr = Alloc<MappedLayout>();
        }

        public static bool HasIntersection(SemanticLayout l, SemanticLayout r)
        {
            return (l._registeredSemantic & r._registeredSemantic) != ShaderSemantic.None;
        }

        public MappedLayout* MapTo<T>(T* pointer) where T : unmanaged
        {
            byte* ptr = (byte*)pointer;
            MappedLayoutPtr->VertexPtr = (Vector4*)(_vertexOffset == OFFSET_NULL ? null : ptr + _vertexOffset);
            MappedLayoutPtr->NormalPtr = (Vector3*)(_normalOffset == OFFSET_NULL ? null : ptr + _normalOffset);
            MappedLayoutPtr->UVPtr = (Vector2*)(_uvOffset == OFFSET_NULL ? null : ptr + _uvOffset);
            MappedLayoutPtr->ColorPtr = (Vector4*)(_colorOffset == OFFSET_NULL ? null : ptr + _colorOffset);
            return MappedLayoutPtr;
        }

        public void Assign(SemanticLayout another)
        {
            ShaderSemantic commonSemantic = _registeredSemantic & another._registeredSemantic;
            if (commonSemantic == ShaderSemantic.None)
                return;

            MappedLayout* anotherLayoutPtr = another.MappedLayoutPtr;
            if (commonSemantic.HasFlag(ShaderSemantic.Vertex))
                Move(anotherLayoutPtr->VertexPtr, MappedLayoutPtr->VertexPtr);
            if (commonSemantic.HasFlag(ShaderSemantic.Normal))
                Move(anotherLayoutPtr->NormalPtr, MappedLayoutPtr->NormalPtr);
            if (commonSemantic.HasFlag(ShaderSemantic.UV))
                Move(anotherLayoutPtr->UVPtr, MappedLayoutPtr->UVPtr);
            if (commonSemantic.HasFlag(ShaderSemantic.Color))
                Move(anotherLayoutPtr->ColorPtr, MappedLayoutPtr->ColorPtr);
        }

        #region Registering

        private const int OFFSET_NULL = -1;

        private bool _isRegistering = false;

        private ShaderSemantic _registeredSemantic;

        private int _vertexOffset = OFFSET_NULL;

        private int _normalOffset = OFFSET_NULL;

        private int _uvOffset = OFFSET_NULL;

        private int _colorOffset = OFFSET_NULL;

        internal void BeginRegister()
        {
            if (_isRegistering)
                throw new Exception("Registering has begun");
            _isRegistering = true;
        }

        internal void RegisterVertex()
        {
            AssertIfNotRegistering();
            _vertexOffset = TotalBufferSize;
            _registeredSemantic |= ShaderSemantic.Vertex;
            TotalBufferSize += sizeof(Vector4);
        }

        internal void RegisterNormal()
        {
            AssertIfNotRegistering();
            _normalOffset = TotalBufferSize;
            _registeredSemantic |= ShaderSemantic.Normal;
            TotalBufferSize += sizeof(Vector3);
        }

        internal void RegisterUV()
        {
            AssertIfNotRegistering();
            _uvOffset = TotalBufferSize;
            _registeredSemantic |= ShaderSemantic.UV;
            TotalBufferSize += sizeof(Vector2);
        }

        internal void RegisterColor()
        {
            AssertIfNotRegistering();
            _colorOffset = TotalBufferSize;
            _registeredSemantic |= ShaderSemantic.Color;
            TotalBufferSize += sizeof(Vector4);
        }

        internal void EndRegister()
        {
            AssertIfNotRegistering();
            _isRegistering = false;
        }

        private void AssertIfNotRegistering()
        {
            if (!_isRegistering)
                throw new Exception("Registering hasn't begun");
        }

        #endregion

        internal void Clear()
        {
            _vertexOffset = OFFSET_NULL;
            _normalOffset = OFFSET_NULL;
            _uvOffset = OFFSET_NULL;
            _colorOffset = OFFSET_NULL;
            TotalBufferSize = 0;
        }

        void IDisposable.Dispose()
        {
            Free(MappedLayoutPtr);
        }
    }
}
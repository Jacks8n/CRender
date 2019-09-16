using System;
using CUtility.Math;

using static CUtility.Extension.MarshalExtension;

namespace CShader
{
    public unsafe partial class SemanticLayout
    {
        public const string SEMANTIC_VERTEX = "Vertex";

        public const string SEMANTIC_NORMAL = "Normal";

        public const string SEMANTIC_UV = "UV";

        public const string SEMANTIC_COLOR = "Color";

        #region Registering

        private void AssertIfNotRegistering()
        {
#if DEBUG
            if (!_isRegistering)
                throw new Exception("Registering hasn't begun");
#endif
        }

        private void AssertIfRegistering()
        {
#if DEBUG
            if (_isRegistering)
                throw new Exception("Registering has begun");
#endif
        }

        public SemanticLayout BeginRegister()
        {
            AssertIfRegistering();
            _isRegistering = true;
            return this;
        }

        private SemanticLayout RegisterVertex()
        {
            AssertIfNotRegistering();
            _vertexOffset = TotalBufferSize;
            RegisteredSemantic |= ShaderSemantic.Vertex;
            TotalBufferSize += sizeof(Vector4);
            return this;
        }

        private SemanticLayout RegisterNormal()
        {
            AssertIfNotRegistering();
            _normalOffset = TotalBufferSize;
            RegisteredSemantic |= ShaderSemantic.Normal;
            TotalBufferSize += sizeof(Vector3);
            return this;
        }

        private SemanticLayout RegisterUV()
        {
            AssertIfNotRegistering();
            _uvOffset = TotalBufferSize;
            RegisteredSemantic |= ShaderSemantic.UV;
            TotalBufferSize += sizeof(Vector2);
            return this;
        }

        private SemanticLayout RegisterColor()
        {
            AssertIfNotRegistering();
            _colorOffset = TotalBufferSize;
            RegisteredSemantic |= ShaderSemantic.Color;
            TotalBufferSize += sizeof(Vector4);
            return this;
        }

        public SemanticLayout RegisterSemantic(bool hasVertex = false, bool hasNormal = false, bool hasUV = false, bool hasColor = false)
        {
            if (hasVertex)
                RegisterVertex();
            if (hasNormal)
                RegisterNormal();
            if (hasUV)
                RegisterUV();
            if (hasColor)
                RegisterColor();
            return this;
        }

        public SemanticLayout RegisterSemantic(ReadOnlySpan<char> semanticString)
        {
            switch (semanticString)
            {
                case ReadOnlySpan<char> str when str.SequenceEqual(SEMANTIC_VERTEX):
                    RegisterVertex();
                    break;
                case ReadOnlySpan<char> str when str.SequenceEqual(SEMANTIC_NORMAL):
                    RegisterNormal();
                    break;
                case ReadOnlySpan<char> str when str.SequenceEqual(SEMANTIC_UV):
                    RegisterUV();
                    break;
                case ReadOnlySpan<char> str when str.SequenceEqual(SEMANTIC_COLOR):
                    RegisterColor();
                    break;
            }
            return this;
        }

        public void EndRegister()
        {
            AssertIfNotRegistering();
            _isRegistering = false;
        }

        #endregion

        public unsafe MappedLayout* MapToValues<T>(T* pointer) where T : unmanaged
        {
            AssertIfRegistering();

            byte* ptr = (byte*)pointer;
            MappedLayoutPtr->VertexPtr = (Vector4*)(_vertexOffset == OFFSET_NULL ? null : ptr + _vertexOffset);
            MappedLayoutPtr->NormalPtr = (Vector3*)(_normalOffset == OFFSET_NULL ? null : ptr + _normalOffset);
            MappedLayoutPtr->UVPtr = (Vector2*)(_uvOffset == OFFSET_NULL ? null : ptr + _uvOffset);
            MappedLayoutPtr->ColorPtr = (Vector4*)(_colorOffset == OFFSET_NULL ? null : ptr + _colorOffset);
            return MappedLayoutPtr;
        }

        public unsafe void AssignValues(SemanticLayout another)
        {
            AssertIfRegistering();

            ShaderSemantic commonSemantic = RegisteredSemantic & another.RegisteredSemantic;
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
    }
}

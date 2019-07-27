using System;
using CUtility.Math;

using static CUtility.Extension.MarshalExtension;

namespace CShader
{
    [Flags]
    internal enum ShaderSemantic { None = 0, Vertex = 1, Normal = 2, UV = 4, Color = 8, All = Vertex | Normal | UV | Color }

    public unsafe class SemanticLayout
    {
        internal ShaderSemantic RegisteredSemantic { get; private set; }

        public Vector4* VertexPtr { get; private set; }

        public Vector3* NormalPtr { get; private set; }

        public Vector2* UVPtr { get; private set; }

        public Vector4* ColorPtr { get; private set; }

        internal int TotalBufferSize;

        private const int OFFSET_NULL = -1;

        private static readonly byte* DiscardBuffer;

        private int _vertexOffset;

        private int _normalOffset;

        private int _uvOffset;

        private int _colorOffset;

        static SemanticLayout()
        {
            DiscardBuffer = AllocBytes<byte>(sizeof(Vector4));
            FreeWhenExit(DiscardBuffer);
        }

        public static bool HasIntersection(SemanticLayout l,SemanticLayout r)
        {
            return (l.RegisteredSemantic & r.RegisteredSemantic) != ShaderSemantic.None;
        }

        public void SetReadWritePointer<T>(T* pointer) where T : unmanaged
        {
            byte* ptr = (byte*)pointer;
            VertexPtr = (Vector4*)(_vertexOffset == OFFSET_NULL ? DiscardBuffer : ptr + _vertexOffset);
            NormalPtr = (Vector3*)(_normalOffset == OFFSET_NULL ? DiscardBuffer : ptr + _normalOffset);
            UVPtr = (Vector2*)(_uvOffset == OFFSET_NULL ? DiscardBuffer : ptr + _uvOffset);
            ColorPtr = (Vector4*)(_colorOffset == OFFSET_NULL ? DiscardBuffer : ptr + _colorOffset);
        }

        public void Assign(SemanticLayout another)
        {
            ShaderSemantic commonSemantic = RegisteredSemantic & another.RegisteredSemantic;
            if (commonSemantic == ShaderSemantic.None)
                return;
            *VertexPtr = *another.VertexPtr;
            *NormalPtr = *another.NormalPtr;
            *UVPtr = *another.UVPtr;
            *ColorPtr = *another.ColorPtr;
        }

        public void SetLayout(Vector4[] vertices, Vector3[] normals, Vector2[] uvs, Vector4[] colors)
        {
            int offset = 0;
            RegisteredSemantic = ShaderSemantic.None;
            if (vertices != null)
            {
                _vertexOffset = offset;
                RegisteredSemantic |= ShaderSemantic.Vertex;
                offset += sizeof(Vector4);
            }
            else
                _vertexOffset = OFFSET_NULL;
            if (normals != null)
            {
                _normalOffset = offset;
                RegisteredSemantic |= ShaderSemantic.Normal;
                offset += sizeof(Vector3);
            }
            else
                _normalOffset = OFFSET_NULL;
            if (uvs != null)
            {
                _uvOffset = offset;
                RegisteredSemantic |= ShaderSemantic.UV;
                offset += sizeof(Vector2);
            }
            else
                _uvOffset = OFFSET_NULL;
            if (colors != null)
            {
                _colorOffset = offset;
                RegisteredSemantic |= ShaderSemantic.Color;
                offset += sizeof(Vector4);
            }
            else
                _colorOffset = OFFSET_NULL;
            TotalBufferSize = offset;
        }

        internal void SetLayout(ShaderSemantic semantic)
        {
            int offset = 0;
            if (semantic.HasFlag(ShaderSemantic.Vertex))
            {
                _vertexOffset = offset;
                offset += sizeof(Vector4);
            }
            else
                _vertexOffset = OFFSET_NULL;
            if (semantic.HasFlag(ShaderSemantic.Normal))
            {
                _normalOffset = offset;
                offset += sizeof(Vector3);
            }
            else
                _normalOffset = OFFSET_NULL;
            if (semantic.HasFlag(ShaderSemantic.UV))
            {
                _uvOffset = offset;
                offset += sizeof(Vector2);
            }
            else
                _uvOffset = OFFSET_NULL;
            if (semantic.HasFlag(ShaderSemantic.Color))
            {
                _colorOffset = offset;
                offset += sizeof(Vector4);
            }
            else
                _colorOffset = OFFSET_NULL;
            TotalBufferSize = offset;
            RegisteredSemantic = semantic;
        }

        internal static int SizeOfSemantic(ShaderSemantic semantic)
        {
            switch (semantic)
            {
                case ShaderSemantic.Vertex:
                case ShaderSemantic.Color:
                    return sizeof(Vector4);
                case ShaderSemantic.Normal:
                    return sizeof(Vector3);
                case ShaderSemantic.UV:
                    return sizeof(Vector2);
            }
            return 0;
        }
    }
}

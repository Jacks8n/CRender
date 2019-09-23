using System.Runtime.InteropServices;
using CUtility.Math;

namespace CShader
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct ShaderInOutPatternDefault
    {
        public Vector4 Vertex;

        public Vector2 UV;

        public Vector3 Normal;

        public Vector4 Color;

        public ShaderInOutPatternDefault(Vector4 vertex, Vector2 uv, Vector3 normal, Vector4 color)
        {
            Vertex = vertex;
            UV = uv;
            Normal = normal;
            Color = color;
        }
    }
}

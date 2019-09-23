using System.Runtime.InteropServices;
using CUtility.Math;

namespace CShader
{
    public static class ShaderInOutDefault
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct App_Base
        {
            public Vector4 Vertex;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct VOut_Base
        {
            public Vector4 Vertex;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FIn_Base
        {
            public Vector4 Vertex;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FOut_Base
        {
            public Vector4 Color;
        }
    }
}

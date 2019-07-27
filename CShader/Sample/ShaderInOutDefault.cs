using CUtility.Math;

namespace CShader
{
    public static class ShaderInOutDefault
    {
        public struct App_Base
        {
            public Vector4 Vertex;
        }

        public struct VOut_Base
        {
            public Vector4 Vertex;
        }

        public struct FIn_Base
        {
            public Vector4 Vertex;
        }

        public struct FOut_Base
        {
            public Vector4 Color;
        }
    }
}

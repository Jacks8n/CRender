using CUtility.Math;

using static CUtility.Extension.MarshalExt;

namespace CShader
{
    public static unsafe class ShaderValue
    {
        public static Matrix4x4* ObjectToWorld;

        public static Matrix4x4* WorldToView;

        public static Matrix4x4* ObjectToView;

        public static float Time;

        public static float SinTime;

        static ShaderValue()
        {
            ObjectToWorld = Alloc<Matrix4x4>(3);
            WorldToView = ObjectToWorld + 1;
            ObjectToView = WorldToView + 1;
            FreeWhenExit(ObjectToWorld);
        }
    }
}

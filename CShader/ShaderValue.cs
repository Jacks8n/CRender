using CUtility.Math;

using static CUtility.Extension.MarshalExtension;

namespace CShader
{
    public static unsafe class ShaderValue
    {
        public static Matrix4x4* ObjectToWorld;

        public static Matrix4x4* WorldToView;

        public static Matrix4x4* ObjectToView;

        public static float Time;

        public static float SinTime;

        public static float SinTime2;

        static ShaderValue()
        {
            ObjectToWorld = Alloc<Matrix4x4>(3);
            WorldToView = ObjectToWorld + 1;
            ObjectToView = WorldToView + 1;
            FreeWhenExit(ObjectToWorld);
        }
    }
}

using CUtility.Math;

using static CUtility.Extension.MarshalExtension;

namespace CShader
{
    public static unsafe class ShaderValue
    {
        public static Matrix4x4* ObjectToWorld { get; private set; }

        public static Matrix4x4* WorldToView { get; private set; }

        public static Matrix4x4* ObjectToView { get; private set; }

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

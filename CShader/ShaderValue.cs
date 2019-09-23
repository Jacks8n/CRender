using CUtility.Math;

using static CUtility.Extension.MarshalExtension;

namespace CShader
{
    public static unsafe class ShaderValue
    {
        public static readonly Matrix4x4* ObjectToWorld;

        public static readonly Matrix4x4* WorldToView;

        public static readonly Matrix4x4* ObjectToView;

        public static readonly Matrix4x4* ObjectToScreen;

        public static float Time;

        public static float SinTime;

        public static float SinTime2;

        public static float CosTime;

        public static float CosTime2;

        static ShaderValue()
        {
            ObjectToWorld = AllocPermanant<Matrix4x4>(4);
            WorldToView = ObjectToWorld + 1;
            ObjectToView = WorldToView + 1;
            ObjectToScreen = ObjectToView + 1;
        }
    }
}

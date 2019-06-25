using CUtility.Math;

using static CUtility.Math.Matrix4x4;

namespace CRender.Structure
{
    public unsafe class Camera_Orthographic : CameraBase
    {
        public override Matrix4x4* WorldToView => Mul(Orthographic(Width, Height, NearPlane, FarPlane, ARITHMETIC_TEMP), Transform.WorldToLocal);

        public Camera_Orthographic(float width, float height, float near, float far, Transform transform) : base(width, height, near, far, transform)
        {

        }
    }
}

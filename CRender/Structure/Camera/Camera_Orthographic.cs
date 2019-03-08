using CRender.Math;

namespace CRender.Structure
{
    public class Camera_Orthographic : CameraBase
    {
        public override Matrix4x4 WorldToView => Matrix4x4.Orthographic(Width, Height, NearPlane, FarPlane) * Transform.WorldToLocal;

        public Camera_Orthographic(float width, float height, float near, float far, Transform transform) : base(width, height, near, far, transform)
        {

        }
    }
}

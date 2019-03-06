using CRender.Math;

namespace CRender.Structure
{
    public class Camera_Orthographic : CameraBase
    {
        public override Matrix4x4 WorldToView => Matrix4x4.Orthographic(Width, Height, NearPlane, FarPlane) * Transform.WorldToLocal;

        public Camera_Orthographic(float height, float width, float near, float far) : base(height, width, near, far)
        {

        }
    }
}

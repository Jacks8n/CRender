using CRender.Math;

namespace CRender.Structure
{
    public abstract class CameraBase : ICamera
    {
        public abstract Matrix4x4 WorldToView { get; }

        public Transform Transform { get; }

        public float Height { get; set; }

        public float Width { get; set; }

        public float NearPlane { get; set; }

        public float FarPlane { get; set; }

        public CameraBase(float height, float width, float near, float far)
        {
            Transform = new Transform(this);
            Height = height;
            Width = width;
            NearPlane = near;
            FarPlane = far;
        }
    }
}

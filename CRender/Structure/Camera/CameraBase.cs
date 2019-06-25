using CUtility.Math;

namespace CRender.Structure
{
    public abstract class CameraBase : ICamera
    {
        public abstract unsafe Matrix4x4* WorldToView { get; }

        public Transform Transform { get; }

        public float Width { get; set; }

        public float Height { get; set; }

        public float NearPlane { get; set; }

        public float FarPlane { get; set; }

        public CameraBase(float width, float height, float near, float far, Transform transform)
        {
            Transform = transform;
            Width = width;
            Height = height;
            NearPlane = near;
            FarPlane = far;
        }
    }
}

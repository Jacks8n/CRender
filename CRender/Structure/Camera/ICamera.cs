using CRender.Math;

namespace CRender.Structure
{
    public interface ICamera : IRenderObject
    {
        Matrix4x4 WorldToView { get; }

        float Height { get; }

        float Width { get; }

        float NearPlane { get; }

        float FarPlane { get; }
    }
}
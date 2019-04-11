using CUtility.Math;

namespace CRender.Structure
{
    public interface ICamera : IRenderObject
    {
        Matrix4x4 WorldToView { get; }

        float Width { get; }

        float Height { get; }

        float NearPlane { get; }

        float FarPlane { get; }
    }
}
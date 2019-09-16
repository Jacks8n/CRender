using CUtility.Math;

namespace CRender.Structure.Light
{
    public interface ILight
    {
        float Intensity { get; set; }

        void LightDirectionAt(Vector3 pos, out Vector3 direction, out float intensity);
    }
}

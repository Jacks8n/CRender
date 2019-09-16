using CUtility.Math;

namespace CRender.Structure.Light
{
    public abstract class LightBase : IRenderObject, ILight
    {
        private const float DEFAULT_INTENSITY = 1f;

        public Transform Transform { get; } = new Transform();

        public float Intensity { get; set; } = DEFAULT_INTENSITY;

        public abstract void LightDirectionAt(Vector3 pos, out Vector3 direction, out float intensity);
    }
}

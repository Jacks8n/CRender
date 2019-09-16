using CUtility.Math;

namespace CRender.Structure.Light
{
    public class DirectionalLight : LightBase
    {
        public override void LightDirectionAt(Vector3 _, out Vector3 direction, out float intensity)
        {
            direction = Transform.Position.Normalized;
            intensity = Intensity;
        }
    }
}

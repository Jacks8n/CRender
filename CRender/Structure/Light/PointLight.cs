using System;
using CUtility.Math;

namespace CRender.Structure.Light
{
    public class PointLight : LightBase
    {
        private const float DEFAULT_RANGE = 1f;

        private const float DEFAULT_SQR_RANGE = DEFAULT_RANGE * DEFAULT_RANGE;

        private const float MIN_RANGE = 1e-5f;

        private const float EPSILON = 1f;

        public float Range
        {
            get => _range;
            set
            {
                _range = MathF.Max(value, MIN_RANGE);
                _sqrRange = _range;
                _invSqrRange = 1f / _sqrRange;
            }
        }

        private float _range = DEFAULT_RANGE;

        private float _sqrRange = DEFAULT_SQR_RANGE;

        private float _invSqrRange = 1f / DEFAULT_SQR_RANGE;

        public override void LightDirectionAt(Vector3 pos, out Vector3 direction, out float intensity)
        {
            direction = pos - Transform.Position;

            float sqrMagnitude = direction.SqrMagnitude;
            if (sqrMagnitude > _sqrRange)
            {
                intensity = 0f;
                return;
            }
            intensity = WindowDistance(sqrMagnitude) / (sqrMagnitude + EPSILON);
        }

        private float WindowDistance(float sqrDistance)
        {
            sqrDistance *= _invSqrRange;
            sqrDistance *= sqrDistance;
            sqrDistance = 1 - sqrDistance;
            return sqrDistance * sqrDistance;
        }
    }
}

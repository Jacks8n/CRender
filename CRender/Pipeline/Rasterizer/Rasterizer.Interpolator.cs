using CUtility.Collection;

namespace CRender.Pipeline
{
    public sealed unsafe partial class Rasterizer
    {
        public static class Interpolator
        {
            public static readonly UnsafeList<float> InterpolatedValues = new UnsafeList<float>();

            private static readonly UnsafeList<float> FromValues = new UnsafeList<float>();

            private static readonly UnsafeList<float> StepValues = new UnsafeList<float>();

            public static void SetRange(float* fromPtr, float* toPtr, int interpolationCount, float range)
            {
                FromValues.EnsureVacant(interpolationCount);
                FromValues.Assign(0, fromPtr, interpolationCount);

                InterpolatedValues.EnsureVacant(interpolationCount);
                InterpolatedValues.Assign(0, fromPtr, interpolationCount);

                StepValues.EnsureVacant(interpolationCount);
                range = 1f / range;
                for (int i = 0; i < interpolationCount; i++)
                    StepValues[i] = (toPtr[i] - fromPtr[i]) * range;
            }

            public static void AccumulateStep()
            {
                for (int i = 0; i < StepValues.Count; i++)
                    InterpolatedValues[i] += StepValues[i];
            }
        }
    }
}

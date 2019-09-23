using System.Runtime.CompilerServices;
using CUtility.Collection;

namespace CRender.Pipeline.Rasterization
{
    public sealed unsafe class Interpolator
    {
        public readonly UnsafeList<float> InterpolatedValues;

        public readonly UnsafeList<float> StepValues;

        public Interpolator(UnsafeList<float> interpolatedValues = null, UnsafeList<float> stepValues = null)
        {
            InterpolatedValues = interpolatedValues ?? new UnsafeList<float>();
            StepValues = stepValues ?? new UnsafeList<float>();
        }

        public void SetInterpolationCount(int count)
        {
            PrepareList(InterpolatedValues, count);
            PrepareList(StepValues, count);
        }

        public void SetInterpolation(float* fromPtr, float* toPtr, int interpolationCount, float range)
        {
            SetInterpolationCount(interpolationCount);
            InterpolatedValues.Assign(0, fromPtr, interpolationCount);

            range = 1f / range;
            for (int i = 0; i < interpolationCount; i++)
                StepValues[i] = (toPtr[i] - fromPtr[i]) * range;
        }

        public void IncrementStep()
        {
            for (int i = 0; i < StepValues.Count; i++)
                InterpolatedValues[i] += StepValues[i];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void PrepareList(UnsafeList<float> list, int interpolationCount)
        {
            list.Clear();
            list.AddEmpty(interpolationCount);
        }
    }
}

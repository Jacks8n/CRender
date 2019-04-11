using System;
using CUtility.Math;

namespace CRender.Pipeline
{
    public static unsafe class Interpolater
    {
        /// <typeparam name="T">Type of structure to contrive result</typeparam>
        /// <param name="from">Start value</param>
        /// <param name="to">End value</param>
        /// <param name="results">Pointer to an array of <typeparamref name="T"/> where results will be assign</param>
        /// <param name="count">The number of interpolation</param>
        public static void Interpolate<T>(IInterpolatable<int> from, IInterpolatable<int> to, T* results, int count) where T : unmanaged, IInterpolatable<int>
        {
            if (from.Length != to.Length)
                throw new Exception("Length of two inputs is different");

            float* steps = stackalloc float[from.Length];
            for (int i = 0; i < from.Length; i++)
                steps[i] = ((float)to.ValuesPtr[i] - from.ValuesPtr[i]) / count;

            while (count-- > 0)
                for (int i = 0; i < from.Length; i++)
                    results[count].ValuesPtr[i] = (int)(steps[i] * i);
        }

        public static void Interpolate<T>(IInterpolatable<float> from, IInterpolatable<float> to, T* results, int count) where T : unmanaged, IInterpolatable<float>
        {
            if (from.Length != to.Length)
                throw new Exception("Length of two inputs is different");

            float* steps = stackalloc float[from.Length];
            for (int i = 0; i < from.Length; i++)
                steps[i] = (to.ValuesPtr[i] - from.ValuesPtr[i]) / count;

            while (count-- > 0)
                for (int i = 0; i < from.Length; i++)
                    results[count].ValuesPtr[i] = steps[i] * i;
        }

        public static void Interpolate<T>(IInterpolatable<Vector2> from, IInterpolatable<Vector2> to, T* results, int count) where T : unmanaged, IInterpolatable<Vector2>
        {
            if (from.Length != to.Length)
                throw new Exception("Length of two inputs is different");

            float* steps = stackalloc float[from.Length * 2];
            float* fromPtr = (float*)from.ValuesPtr;
            float* toPtr = (float*)to.ValuesPtr;
            for (int i = 0; i < from.Length; i++)
            {
                steps[i++] = (toPtr[i] - fromPtr[i]) / count;
                steps[i] = (toPtr[i] - fromPtr[i]) / count;
            }

            while (count-- > 0)
                for (int i = 0; i < from.Length; i++)
                    results[count].ValuesPtr[i] = new Vector2(steps[i * 2] * i, steps[i * 2 + 1] * i);
        }
    }
}
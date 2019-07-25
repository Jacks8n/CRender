using System;

namespace CUtility.Math
{
    public static class JMath
    {
        public const float PI_TWO = 6.283185307179586476925286766559f;

        public const float PI = 3.1415926535897932384626433832795f;

        public const float PI_HALF = 1.5707963267948966192313216916398f;

        public const float SQRT2 = 1.414213562373095f;

        public const float SQRT3 = 1.732050807568877f;

        public const float EPSILON = 1e-5f;

        private const float REALLY_BIG_FUCKING_NUMBER = 1e8f;

        public static Vector3 Sin(Vector3 vector)
        {
            return new Vector3(MathF.Sin(vector.X), MathF.Sin(vector.Y), MathF.Sin(vector.Z));
        }

        public static Vector3 Cos(Vector3 vector)
        {
            return new Vector3(MathF.Cos(vector.X), MathF.Cos(vector.Y), MathF.Cos(vector.Z));
        }

        public static float Cap(float value)
        {
            return value > REALLY_BIG_FUCKING_NUMBER ? REALLY_BIG_FUCKING_NUMBER : value;
        }

        /// <summary>
        /// If <paramref name="left"/> approximates<paramref name="right"/> w.r.t. <see cref="EPSILON"/>
        /// </summary>
        public static bool Approx(float left, float right)
        {
            return left - right < EPSILON && left - right > -EPSILON;
        }

        public static int ApproxSign(float value)
        {
            return value < -EPSILON ? -1 : value > EPSILON ? 1 : 0;
        }

        /// <summary>
        /// Exclusive
        /// </summary>
        public static bool InRange(float value, float min, float max)
        {
            return value > min && value < max;
        }

        public static float Ratio(float value, float from, float to)
        {
            return (value - from) / (to - from);
        }

        public static float Lerp(float value, float from, float to)
        {
            return (to - from) * value + from;
        }

        public static GenericVector<T> Lerp<T>(float value, GenericVector<T> from, GenericVector<T> to) where T : unmanaged
        {
            if (from.Length != to.Length)
                throw new Exception("Two inputs' length differs");

            GenericVector<T> result = new GenericVector<T>(from.Length);

            if (typeof(T) == typeof(float))
                for (int i = 0; i < from.Length; i++)
                    result[i] = (T)(object)Lerp(value, (float)(object)from[i], (float)(object)to[i]);
            return result;
        }

        public static float Floor(float value, float precision)
        {
            return value - (value % precision);
        }
        public static int RoundToInt(float value)
        {
            return value % 1f <= .5f ? (int)value : (int)value + 1;
        }
    }
}

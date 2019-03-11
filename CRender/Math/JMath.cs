using System;
using CRender.Structure;

namespace CRender.Math
{
    public static class JMath
    {
        public const float PI_TWO = 6.283185307179586476925286766559f;

        public const float PI = 3.1415926535897932384626433832795f;

        public const float PI_HALF = 1.5707963267948966192313216916398f;

        public static Vector3 Sin(Vector3 vector)
        {
            return new Vector3(MathF.Sin(vector.X), MathF.Sin(vector.Y), MathF.Sin(vector.Z));
        }

        public static Vector3 Cos(Vector3 vector)
        {
            return new Vector3(MathF.Cos(vector.X), MathF.Cos(vector.Y), MathF.Cos(vector.Z));
        }

        /// <summary>
        /// Exclusive
        /// </summary>
        public static bool InRange(float value,float min,float max)
        {
            return value > min && value < max;
        }

        public static float Lerp(float value, float from, float to)
        {
            return (to - from) * value + from;
        }

        public static float CoLerp(float value, float from, float to, float min, float max)
        {
            return (max - min) * (value / (to - from)) + min;
        }

        public static float Floor(float value, float precision)
        {
            return value - (value % precision);
        }

        public static GenericVector<float> Lerp(float value, GenericVector<float> from, GenericVector<float> to)
        {
            for (int i = 0; i < from.Length; i++)
                from[i] = Lerp(value, from[i], to[i]);
            return from;
        }
    }
}

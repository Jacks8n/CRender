﻿using System;

namespace CUtility.Math
{
    [System.Diagnostics.DebuggerDisplay("X: {X} Y: {Y} Z: {Z}")]
    public struct Vector3
    {
        #region Constants

        public static readonly Vector3 Zero = new Vector3(0, 0, 0);

        public static readonly Vector3 One = new Vector3(1, 1, 1);

        public static readonly Vector3 UnitXPositive = new Vector3(1, 0, 0);

        public static readonly Vector3 UnitXNegative = new Vector3(-1, 0, 0);

        public static readonly Vector3 UnitYPositive = new Vector3(0, 1, 0);

        public static readonly Vector3 UnitYNegative = new Vector3(0, -1, 0);

        public static readonly Vector3 UnitZPositive = new Vector3(0, 0, 1);

        public static readonly Vector3 UnitZNegative = new Vector3(0, 0, -1);

        public static readonly Vector3 MaxValue = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

        public static readonly Vector3 MinValue = new Vector3(float.MinValue, float.MinValue, float.MinValue);

        #endregion

        public float X, Y, Z;

        public float SqrMagnitude => X * X + Y * Y + Z * Z;

        public float Magnitude => MathF.Sqrt(SqrMagnitude);

        public Vector3 Normalized => this * (1f / Magnitude);

        public Vector3(float xyz)
        {
            X = Y = Z = xyz;
        }

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString() => $"{{ X:{X} Y:{Y} Z:{Z} }}";

        public static Vector3 operator -(Vector3 value) => new Vector3(-value.X, -value.Y, -value.Z);

        public static Vector3 operator +(Vector3 l, Vector3 r) => new Vector3(l.X + r.X, l.Y + r.Y, l.Z + r.Z);

        public static Vector3 operator -(Vector3 l, Vector3 r) => new Vector3(l.X - r.X, l.Y - r.Y, l.Z - r.Z);

        public static Vector3 operator *(Vector3 value, float multiplier) => new Vector3(value.X * multiplier, value.Y * multiplier, value.Z * multiplier);

        public static Vector3 operator /(Vector3 value, float divisor)
        {
            divisor = 1f / divisor;
            return new Vector3(value.X * divisor, value.Y * divisor, value.Z * divisor);
        }
    }
}

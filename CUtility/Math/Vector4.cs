using System.Runtime.InteropServices;

namespace CUtility.Math
{
    [System.Diagnostics.DebuggerDisplay("X:{X} Y:{Y} Z:{Z} W:{W}")]
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector4
    {
        #region Constants

        public static readonly Vector4 Zero = new Vector4(0, 0, 0, 0);

        public static readonly Vector4 One = new Vector4(1, 1, 1, 1);

        public static readonly Vector4 UnitXPositive_Point = new Vector4(Vector3.UnitXPositive, 1);

        public static readonly Vector4 UnitXNegative_Point = new Vector4(Vector3.UnitXNegative, 1);

        public static readonly Vector4 UnitYPositive_Point = new Vector4(Vector3.UnitYPositive, 1);

        public static readonly Vector4 UnitYNegative_Point = new Vector4(Vector3.UnitYNegative, 1);

        public static readonly Vector4 UnitZPositive_Point = new Vector4(Vector3.UnitZPositive, 1);

        public static readonly Vector4 UnitZNegative_Point = new Vector4(Vector3.UnitZNegative, 1);

        public static readonly Vector4 UnitXPositive_Vector = new Vector4(Vector3.UnitXPositive, 0);

        public static readonly Vector4 UnitXNegative_Vector = new Vector4(Vector3.UnitXNegative, 0);

        public static readonly Vector4 UnitYPositive_Vector = new Vector4(Vector3.UnitYPositive, 0);

        public static readonly Vector4 UnitYNegative_Vector = new Vector4(Vector3.UnitYNegative, 0);

        public static readonly Vector4 UnitZPositive_Vector = new Vector4(Vector3.UnitZPositive, 0);

        public static readonly Vector4 UnitZNegative_Vector = new Vector4(Vector3.UnitZNegative, 0);

        #endregion

        public Vector2 XY => new Vector2(X, Y);

        public Vector2 XZ => new Vector2(X, Z);

        public Vector2 YZ => new Vector2(Y, Z);

        public Vector3 XYZ => new Vector3(X, Y, Z);

        public float X, Y, Z, W;

        public Vector4(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public Vector4(Vector3 vector, float w) : this(vector.X, vector.Y, vector.Z, w)
        {
        }

        public static Vector4 operator -(Vector4 value) => new Vector4(-value.X, -value.Y, -value.Z, -value.W);

        public static Vector4 operator +(Vector4 l, Vector4 r) => new Vector4(l.X + r.X, l.Y + r.Y, l.Z + r.Z, r.W + l.W);

        public static Vector4 operator +(Vector4 l, float r) => new Vector4(l.X + r, l.Y + r, l.Z + r, l.W + r);

        public static Vector4 operator -(Vector4 l, Vector4 r) => new Vector4(l.X - r.X, l.Y - r.Y, l.Z - r.Z, r.W - l.W);

        public static Vector4 operator *(Vector4 value, float scale) => new Vector4(value.X * scale, value.Y * scale, value.Z * scale, value.W * scale);

        public static Vector4 operator *(Matrix4x4 l, Vector4 r) => new Vector4(
                x: r.X * l.M11 + r.Y * l.M21 + r.Z * l.M31 + r.W * l.M41,
                y: r.X * l.M12 + r.Y * l.M22 + r.Z * l.M32 + r.W * l.M42,
                z: r.X * l.M13 + r.Y * l.M23 + r.Z * l.M33 + r.W * l.M43,
                w: r.X * l.M14 + r.Y * l.M24 + r.Z * l.M34 + r.W * l.M44);

        public override string ToString() => $"{{ X:{X} Y:{Y} Z:{Z} W:{W} }}";
    }
}

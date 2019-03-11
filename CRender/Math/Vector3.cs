namespace CRender.Math
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

        #endregion

        public float X, Y, Z;

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

        public static Vector3 operator *(Vector3 value, float scale) => new Vector3(value.X * scale, value.Y * scale, value.Z * scale);
    }
}

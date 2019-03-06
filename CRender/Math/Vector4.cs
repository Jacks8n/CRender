namespace CRender.Math
{
    [System.Diagnostics.DebuggerDisplay("X:{X} Y:{Y} Z:{Z} W:{W}")]
    public struct Vector4
    {
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

        public override string ToString() => $"{{ X:{X} Y:{Y} Z:{Z} W:{W} }}";

        public static Vector4 operator -(Vector4 value) => new Vector4(-value.X, -value.Y, -value.Z, -value.W);

        public static Vector4 operator +(Vector4 l, Vector4 r) => new Vector4(l.X + r.X, l.Y + r.Y, l.Z + r.Z, r.W + l.W);

        public static Vector4 operator -(Vector4 l, Vector4 r) => new Vector4(l.X - r.X, l.Y - r.Y, l.Z - r.Z, r.W - l.W);

        public static Vector4 operator *(Vector4 value, float scale) => new Vector4(value.X * scale, value.Y * scale, value.Z * scale, value.W * scale);
    }
}

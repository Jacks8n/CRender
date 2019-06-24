using System.Runtime.InteropServices;

namespace CUtility.Math
{
    [System.Diagnostics.DebuggerDisplay("X: {X} Y: {Y}")]
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector2
    {
        public static readonly Vector2 Zero = new Vector2(0, 0);

        public static readonly Vector2 One = new Vector2(1, 1);

        public float X, Y;

        public float this[int index]
        {
            get => index == 0 ? X : Y;
            set
            {
                switch (index)
                {
                    case 0:
                        X = value;
                        break;
                    case 1:
                        Y = value;
                        break;
                }
            }
        }

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static Vector2 operator +(Vector2 l, Vector2 r) => new Vector2(l.X + r.X, l.Y + r.Y);

        public static Vector2 operator -(Vector2 l, Vector2 r) => new Vector2(l.X - r.X, l.Y - r.Y);

        public static Vector2 operator *(Vector2 vector, float scale) => new Vector2(vector.X * scale, vector.Y * scale);

        public static Vector2 operator *(Vector2 l, Vector2 r) => new Vector2(l.X * r.X, l.Y * r.Y);

        public static Vector2 operator /(Vector2 vector, float scale) => new Vector2(vector.X / scale, vector.Y / scale);

        public static bool operator ==(Vector2 l, Vector2 r) => l.X == r.X && l.Y == r.Y;

        public static bool operator !=(Vector2 l, Vector2 r) => l.X != r.X || l.Y != r.Y;

        public static explicit operator Vector2(Vector2Int vector) => new Vector2(vector.X, vector.Y);

        public override string ToString() => $"{{X:{X} Y:{Y}}}";
    }
}

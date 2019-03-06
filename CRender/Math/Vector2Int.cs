namespace CRender.Math
{
    [System.Diagnostics.DebuggerDisplay("X: {X} Y: {Y}")]
    public struct Vector2Int
    {
        public static readonly Vector2Int Zero = new Vector2Int(0, 0);

        public int X, Y;

        public int this[int index]
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

        public Vector2Int(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static bool operator ==(Vector2Int l, Vector2Int r) => l.X == r.X && l.Y == r.Y;

        public static bool operator !=(Vector2Int l, Vector2Int r) => l.X != r.X || l.Y != r.Y;

        public static explicit operator Vector2Int(Vector2 vector) => new Vector2Int((int)vector.X, (int)vector.Y);

        public override string ToString() => $"{{X:{X} Y:{Y}}}";
    }
}
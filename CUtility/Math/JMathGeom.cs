using MathLib = System.Math;

namespace CUtility.Math
{
    public static class JMathGeom
    {
        public static Cuboid GetBoundBox(Vector3[] vertices)
        {
            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float minZ = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;
            float maxZ = float.MinValue;
            for (int i = 0; i < vertices.Length; i++)
            {
                minX = MathLib.Min(vertices[i].X, minX);
                minY = MathLib.Min(vertices[i].Y, minY);
                minZ = MathLib.Min(vertices[i].Z, minZ);
                maxX = MathLib.Max(vertices[i].X, maxX);
                maxY = MathLib.Max(vertices[i].Y, maxY);
                maxZ = MathLib.Max(vertices[i].Z, maxZ);
            }
            return new Cuboid(maxX - minX, maxY - minY, maxZ - minZ,
                pos: new Vector3((minX + maxX) * .5f, (minY + maxY) * .5f, (minZ + maxZ) * .5f));
        }

        /// <summary>
        /// -1: left 0:lined 1:right
        /// </summary>
        public static int PointBelongedSide(Vector2 point, Vector2 from, Vector2 to)
        {
            return MathLib.Sign((point.X - from.X) * (to.X - from.X) + (point.Y - from.Y) * (to.Y - from.Y));
        }

        public static Vector2 FloorVector(Vector2 vector, Vector2 precision)
        {
            return new Vector2(JMath.Floor(vector.X, precision.X), JMath.Floor(vector.Y, precision.Y));
        }

        public static float Slope(Vector2 from, Vector2 to)
        {
            return from.Y == to.Y ?
                       from.Y > to.Y ?
                           int.MinValue : int.MaxValue :
                   (from.Y - to.Y) / (from.X - to.X);
        }

        /// <summary>
        /// -1: <paramref name="l"/> is higher than <paramref name="r"/> 1: vice versa 0: overlapped
        /// </summary>
        public static int LexicoCompareRightDown(Vector2 l, Vector2 r)
        {
            return l.X > r.X ? 1 :
                   l.X < r.X ? -1 :
                   l.Y < r.Y ? 1 :
                   l.Y > r.Y ? -1 : 0;
        }

        /// <summary>
        /// -1: <paramref name="l"/> is higher than <paramref name="r"/> 1: vice versa 0: overlapped
        /// </summary>
        public static int LexicoCompareDownRight(Vector2 l, Vector2 r)
        {
            return l.Y < r.Y ? 1 :
                   l.Y > r.Y ? -1 :
                   l.X > r.X ? 1 :
                   l.X < r.X ? -1 : 0;
        }
    }
}

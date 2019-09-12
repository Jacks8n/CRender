using MathLib = System.Math;

namespace CUtility.Math
{
    public struct Cube
    {
        public float Size;

        public Vector3 Position;

        public Cube(float size, Vector3 pos)
        {
            Size = size;
            Position = pos;
        }

        public bool IsIntersect(Cube other)
        {
            float dis = Size + other.Size;
            return MathLib.Abs(other.Position.X - Position.X) < dis
                && MathLib.Abs(other.Position.Y - Position.Y) < dis
                && MathLib.Abs(other.Position.Z - Position.Z) < dis;
        }
    }

    public struct Cuboid
    {
        public readonly static Cuboid NegativeMax = new Cuboid(Vector3.MaxValue, Vector3.MinValue);

        public float MinX { get => _minVertex.X; set => _minVertex.X = value; }

        public float MinY { get => _minVertex.Y; set => _minVertex.Y = value; }

        public float MinZ { get => _minVertex.Z; set => _minVertex.Z = value; }

        public float MaxX { get => _maxVertex.X; set => _maxVertex.X = value; }

        public float MaxY { get => _maxVertex.Y; set => _maxVertex.Y = value; }

        public float MaxZ { get => _maxVertex.Z; set => _maxVertex.Z = value; }

        private Vector3 _maxVertex;

        private Vector3 _minVertex;

        public Cuboid(float minX, float minY, float minZ, float maxX, float maxY, float maxZ) : this()
        {
            _maxVertex = new Vector3(maxX, maxY, maxZ);
            _minVertex = new Vector3(minX, minY, minZ);
        }

        public Cuboid(Vector3 minVertex, Vector3 maxVertex)
        {
            _maxVertex = maxVertex;
            _minVertex = minVertex;
        }

        public void ExtendToContain(Vector3 point)
        {
            if (point.X > MaxX)
                MaxX = point.X;
            else if (point.X < MinX)
                MinX = point.X;
            if (point.Y > MaxY)
                MaxY = point.Y;
            else if (point.Y < MinY)
                MinY = point.Y;
            if (point.Z > MaxZ)
                MaxZ = point.Z;
            else if (point.Z < MinZ)
                MinZ = point.Z;
        }
    }
}

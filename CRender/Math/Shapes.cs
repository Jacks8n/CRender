using MathLib = System.Math;

namespace CRender.Math
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
        public float LengthX, LengthY, LengthZ;

        public Vector3 Position;
        
        public Cuboid(float x, float y, float z, Vector3 pos)
        {
            LengthX = x;
            LengthY = y;
            LengthZ = z;
            Position = pos;
        }
    }
}

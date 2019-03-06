using CRender.Math;

namespace CRender.Structure
{
    public struct Model : IAppliable<Model>
    {
        public readonly Vector3[] Vertices;

        public readonly IPrimitive[] Primitives;

        public readonly Vector2[] UVs;

        public readonly Vector3[] Normals;

        public readonly Cuboid Bound;

        public Model(Vector3[] vertices, IPrimitive[] triangleIndices, Vector2[] uvs, Vector3[] normals)
            : this(vertices, triangleIndices, uvs, normals, JMathGeom.GetBoundBox(vertices))
        {
        }

        private Model(Vector3[] vertices, IPrimitive[] triangleIndices, Vector2[] uvs, Vector3[] normals, Cuboid bound)
        {
            Vertices = vertices;
            Primitives = triangleIndices;
            UVs = uvs;
            Normals = normals;
            Bound = bound;
        }

        public Model GetInstanceToApply()
        {
            return new Model(Vertices.GetCopy(), Primitives.GetCopy(), UVs.GetCopy(), Normals.GetCopy(), Bound);
        }
    }
}
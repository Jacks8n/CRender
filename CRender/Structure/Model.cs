using System;
using CUtility.Math;
using CUtility.Extension;

namespace CRender.Structure
{
    public struct Model : IAppliable<Model>
    {
        public readonly  Vector4[] Vertices;

        public readonly IPrimitive[] Primitives;

        public readonly Vector2[] UVs;

        public readonly Vector3[] Normals;

        public Model(Vector4[] vertices, IPrimitive[] primitives, Vector2[] uvs, Vector3[] normals)//, Cuboid bound)
        {
            Vertices = vertices;
            Primitives = primitives;
            UVs = uvs;
            Normals = normals;
        }

        public Model GetInstanceToApply()
        {
            return new Model(Vertices.GetCopy(), Primitives, UVs?.GetCopy(), Normals?.GetCopy());//, Bound);
        }
    }
}
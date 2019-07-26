using System;
using CUtility.Math;
using CUtility.Extension;
using CUtility.Collection;
using static CUtility.Extension.MarshalExt;

namespace CRender.Structure
{
    public struct Model
    {
        public Vector4[] Vertices;

        public IPrimitive[] Primitives;

        public Vector2[] UVs;

        public Vector3[] Normals;

        public int VerticesDataCount;

        public GenericVector<float>[] VerticesData;

        public unsafe Model(Vector4[] vertices, IPrimitive[] primitives, Vector2[] uvs = null, Vector3[] normals = null, GenericVector<float>[] verticesData = null)
        {
            Vertices = vertices;
            Primitives = primitives;
            UVs = uvs;
            Normals = normals;
            VerticesDataCount = (uvs == null ? 0 : 2)
                + (normals == null ? 0 : 3)
                + (verticesData == null ? 0 : verticesData[0].Length);
            VerticesData = verticesData;
        }

        /// <summary>
        /// A cube without uv mapping
        /// </summary>
        public static Model Cube(float size = 1f, bool isWireframe = true, bool hasNormals = false)
        {
            size *= .5f;
            return new Model(
                vertices: new Vector4[] {
                    new Vector4(-size, -size, -size, 1f),
                    new Vector4(size, -size, -size, 1f),
                    new Vector4(size, size, -size, 1f),
                    new Vector4(-size, size, -size, 1f),
                    new Vector4(-size, -size, size, 1f),
                    new Vector4(size, -size, size, 1f),
                    new Vector4(size, size, size, 1f),
                    new Vector4(-size, size, size, 1f), },

                primitives: isWireframe ?
                new IPrimitive[] {
                    new LinePrimitive(0, 1),
                    new LinePrimitive(1, 2),
                    new LinePrimitive(2, 3),
                    new LinePrimitive(3, 0),
                    new LinePrimitive(0, 4),
                    new LinePrimitive(1, 5),
                    new LinePrimitive(2, 6),
                    new LinePrimitive(3, 7),
                    new LinePrimitive(4, 5),
                    new LinePrimitive(5, 6),
                    new LinePrimitive(6, 7),
                    new LinePrimitive(4, 7), }
                : new IPrimitive[] {
                    new TrianglePrimitive(0, 1, 2),
                    new TrianglePrimitive(2, 3, 0),
                    new TrianglePrimitive(0, 1, 5),
                    new TrianglePrimitive(1, 2, 6),
                    new TrianglePrimitive(2, 3, 7),
                    new TrianglePrimitive(3, 0, 4),
                    new TrianglePrimitive(4, 5, 6),
                    new TrianglePrimitive(6, 7, 4),
                    new TrianglePrimitive(4, 5, 0),
                    new TrianglePrimitive(5, 6, 1),
                    new TrianglePrimitive(6, 7, 2),
                    new TrianglePrimitive(7, 4, 3), },

                uvs: null,

                normals: hasNormals ?
                new Vector3[] {
                    new Vector3(-JMath.SQRT3),
                    new Vector3(JMath.SQRT3, -JMath.SQRT3, -JMath.SQRT3),
                    new Vector3(JMath.SQRT3, JMath.SQRT3, -JMath.SQRT3),
                    new Vector3(-JMath.SQRT3, JMath.SQRT3, -JMath.SQRT3),
                    new Vector3(-JMath.SQRT3, -JMath.SQRT3, JMath.SQRT3),
                    new Vector3(JMath.SQRT3, -JMath.SQRT3, JMath.SQRT3),
                    new Vector3(JMath.SQRT3),
                    new Vector3(-JMath.SQRT3, JMath.SQRT3, JMath.SQRT3), }
                : null
                );
        }

        public static Model Plane(float size = 1f)
        {
            size *= .5f;
            return new Model(
                vertices: new Vector4[]
                {
                    new Vector4(size, size, 0f, 1f),
                    new Vector4(size, -size, 0f, 1f),
                    new Vector4(-size, -size, 0f, 1f),
                    new Vector4(-size, size, 0f, 1f)
                },
                primitives: new IPrimitive[]
                {
                    new TrianglePrimitive(0, 1, 2),
                    new TrianglePrimitive(0, 2, 3),
                },
                uvs: null, normals: null);
        }
    }
}
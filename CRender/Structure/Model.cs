using System;
using CShader;
using CUtility.Math;
using CUtility.Extension;
using CUtility.Collection;
using static CUtility.Extension.MarshalExtension;
using System.Runtime.CompilerServices;

namespace CRender.Structure
{
    public class Model
    {
        public readonly SemanticLayout VerticesDataReader;

        public Vector4[] Vertices { get; private set; }

        public IPrimitive[] Primitives { get; private set; }

        public Vector3[] Normals { get; private set; }

        public Vector2[] UVs { get; private set; }

        public int VerticesDataCount { get; private set; }

        public GenericVector<float>[] VerticesData { get; private set; }

        public unsafe Model(Vector4[] vertices, IPrimitive[] primitives, Vector3[] normals = null, Vector2[] uvs = null)
        {
            VerticesDataReader = new SemanticLayout();
            VerticesDataReader.SetLayout(vertices, normals, uvs, null);
            Vertices = vertices ?? throw new Exception();
            Primitives = primitives;
            UVs = uvs;
            Normals = normals;
            VerticesDataCount = 4
                              + (uvs == null ? 0 : 2)
                              + (normals == null ? 0 : 3);
            VerticesData = new GenericVector<float>[vertices.Length];
            for (int i = 0; i < VerticesData.Length; i++)
            {
                VerticesData[i] = new GenericVector<float>(VerticesDataCount);
                VerticesData[i].Add(vertices[i]);
                if (normals != null)
                    VerticesData[i].Add(normals[i]);
                if (uvs != null)
                    VerticesData[i].Add(uvs[i]);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal unsafe SemanticLayout ReadVertexData(int index)
        {
            VerticesDataReader.SetReadWritePointer(VerticesData[index].ElementsPtr);
            return VerticesDataReader;
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
,

                uvs: null);
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
                normals: null, uvs: null);
        }
    }
}
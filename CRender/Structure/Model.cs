using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using CShader;
using CUtility.Collection;
using CUtility.Extension;
using CUtility.Math;

using static CUtility.Extension.MarshalExtension;

namespace CRender.Structure
{
    public class Model
    {
        public Vector3[] Vertices { get; private set; }

        public int[] Indices { get; private set; }

        public Vector2[] UVs { get; private set; }

        public Vector3[] Normals { get; private set; }

        public Vector4[] Colors { get; private set; }

        internal Cuboid Bound { get; private set; }

        /// <summary>
        /// Vertices data layout matches the one of <see cref="ShaderInOutPatternDefault"/>
        /// </summary>
        private GenericVector<float>[] _verticesData { get; set; }

        [SuppressMessage("Style", "IDE0016:Use 'throw' expression", Justification = "<Pending>")]
        public unsafe Model(Vector3[] vertices, int[] indices, Vector2[] uvs = null, Vector3[] normals = null, Vector4[] colors = null)
        {
            Vertices = vertices ?? throw new Exception("Vertices are required");
            Indices = indices;
            UVs = uvs;
            Normals = normals;
            Colors = colors;

            _verticesData = new GenericVector<float>[vertices.Length];
            Bound = Cuboid.NegativeMax;
            for (int i = 0; i < _verticesData.Length; i++)
            {
                _verticesData[i] = new GenericVector<float>(sizeof(ShaderInOutPatternDefault) / sizeof(float)) {
                    vertices[i], 1f,
                    uvs != null ? uvs[i] : Vector2.Zero,
                    normals != null ? normals[i] : Vector3.Zero,
                    colors != null ? colors[i] : Vector4.Zero };
                Bound.ExtendToContain(vertices[i]);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe ShaderInOutPatternDefault* ReadVerticesDataAsPattern(int index)
        {
            return (ShaderInOutPatternDefault*)_verticesData[index].ElementsPtr;
        }

        /// <summary>
        /// A cube without uv mapping
        /// </summary>
        public static Model Cube(float size = 1f, bool hasNormals = false)
        {
            size *= .5f;
            return new Model(
                vertices: new Vector3[] {
                    new Vector3(-size, -size, -size),
                    new Vector3(size, -size, -size),
                    new Vector3(size, size, -size),
                    new Vector3(-size, size, -size),
                    new Vector3(-size, -size, size),
                    new Vector3(size, -size, size),
                    new Vector3(size, size, size),
                    new Vector3(-size, size, size), },

                indices: new int[] {
                    0, 1, 2,
                    2, 3, 0,
                    0, 1, 5,
                    1, 2, 6,
                    2, 3, 7,
                    3, 0, 4,
                    4, 5, 6,
                    6, 7, 4,
                    4, 5, 0,
                    5, 6, 1,
                    6, 7, 2,
                    7, 4, 3, },

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
                vertices: new Vector3[]
                {
                    new Vector3(size, size, 0f),
                    new Vector3(size, -size, 0f),
                    new Vector3(-size, -size, 0f),
                    new Vector3(-size, size, 0f)
                },
                indices: new int[]
                {
                    0, 1, 2,
                    0, 2, 3,
                },
                uvs: null, normals: null);
        }
    }
}
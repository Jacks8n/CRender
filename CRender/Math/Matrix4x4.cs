namespace CRender.Math
{
    public partial struct Matrix4x4
    {
        public float M11 => M[0][0];
        public float M12 => M[0][1];
        public float M13 => M[0][2];
        public float M14 => M[0][3];
        public float M21 => M[1][0];
        public float M22 => M[1][1];
        public float M23 => M[1][2];
        public float M24 => M[1][3];
        public float M31 => M[2][0];
        public float M32 => M[2][1];
        public float M33 => M[2][2];
        public float M34 => M[2][3];
        public float M41 => M[3][0];
        public float M42 => M[3][1];
        public float M43 => M[3][2];
        public float M44 => M[3][3];

        /// <summary>
        /// M(x, y) -> M[x - 1][y - 1]
        /// </summary>
        public readonly float[][] M;

        public Matrix4x4(float m11, float m21, float m31, float m41,
                         float m12, float m22, float m32, float m42,
                         float m13, float m23, float m33, float m43,
                         float m14, float m24, float m34, float m44)
        {
            M = new float[][] {
                new float[]{ m11, m12, m13, m14},
                new float[]{ m21, m22, m23, m24},
                new float[]{ m31, m32, m33, m34},
                new float[]{ m41, m42, m43, m44},
            };
        }

        public static Matrix4x4 operator *(Matrix4x4 l, Matrix4x4 r) => new Matrix4x4(
                m11: l.M11 * r.M11 + l.M21 * r.M12 + l.M31 * r.M13 + l.M41 * r.M14,
                m12: l.M12 * r.M11 + l.M22 * r.M12 + l.M32 * r.M13 + l.M42 * r.M14,
                m13: l.M13 * r.M11 + l.M23 * r.M12 + l.M33 * r.M13 + l.M43 * r.M14,
                m14: l.M14 * r.M11 + l.M24 * r.M12 + l.M34 * r.M13 + l.M44 * r.M14,

                m21: l.M11 * r.M21 + l.M21 * r.M22 + l.M31 * r.M23 + l.M41 * r.M24,
                m22: l.M12 * r.M21 + l.M22 * r.M22 + l.M32 * r.M23 + l.M42 * r.M24,
                m23: l.M13 * r.M21 + l.M23 * r.M22 + l.M33 * r.M23 + l.M43 * r.M24,
                m24: l.M14 * r.M21 + l.M24 * r.M22 + l.M34 * r.M23 + l.M44 * r.M24,

                m31: l.M11 * r.M31 + l.M21 * r.M32 + l.M31 * r.M33 + l.M41 * r.M34,
                m32: l.M12 * r.M31 + l.M22 * r.M32 + l.M32 * r.M33 + l.M42 * r.M34,
                m33: l.M13 * r.M31 + l.M23 * r.M32 + l.M33 * r.M33 + l.M43 * r.M34,
                m34: l.M14 * r.M31 + l.M24 * r.M32 + l.M34 * r.M33 + l.M44 * r.M34,

                m41: l.M11 * r.M41 + l.M21 * r.M42 + l.M31 * r.M43 + l.M41 * r.M44,
                m42: l.M12 * r.M41 + l.M22 * r.M42 + l.M32 * r.M43 + l.M42 * r.M44,
                m43: l.M13 * r.M41 + l.M23 * r.M42 + l.M33 * r.M43 + l.M43 * r.M44,
                m44: l.M14 * r.M41 + l.M24 * r.M42 + l.M34 * r.M43 + l.M44 * r.M44);

        public static Vector4 operator *(Matrix4x4 l, Vector4 r) => new Vector4(
                x: r.X * l.M11 + r.Y * l.M21 + r.Z * l.M31 + r.W * l.M41,
                y: r.X * l.M12 + r.Y * l.M22 + r.Z * l.M32 + r.W * l.M42,
                z: r.X * l.M13 + r.Y * l.M23 + r.Z * l.M33 + r.W * l.M43,
                w: r.X * l.M14 + r.Y * l.M24 + r.Z * l.M34 + r.W * l.M44);

        public override string ToString()
        {
            return $"M11:{M11} M21:{M21} M31:{M31} M41:{M41}\nM12:{M12} M22:{M22} M32:{M32} M42:{M42}\nM13:{M13} M23:{M23} M33:{M33} M43:{M43}\nM14:{M14} M24:{M24} M34:{M34} M44:{M44}";
        }
    }
}
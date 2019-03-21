namespace CRender.Math
{
    public partial struct Matrix4x4
    {
        public float M11, M21, M31, M41;
        public float M12, M22, M32, M42;
        public float M13, M23, M33, M43;
        public float M14, M24, M34, M44;

        public Matrix4x4(float m11, float m21, float m31, float m41,
                         float m12, float m22, float m32, float m42,
                         float m13, float m23, float m33, float m43,
                         float m14, float m24, float m34, float m44)
        {
            M11 = m11; M21 = m21; M31 = m31; M41 = m41;
            M12 = m12; M22 = m22; M32 = m32; M42 = m42;
            M13 = m13; M23 = m23; M33 = m33; M43 = m43;
            M14 = m14; M24 = m24; M34 = m34; M44 = m44;
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

        public override string ToString()
        {
            return $"M11:{M11} M21:{M21} M31:{M31} M41:{M41}\nM12:{M12} M22:{M22} M32:{M32} M42:{M42}\nM13:{M13} M23:{M23} M33:{M33} M43:{M43}\nM14:{M14} M24:{M24} M34:{M34} M44:{M44}";
        }
    }
}
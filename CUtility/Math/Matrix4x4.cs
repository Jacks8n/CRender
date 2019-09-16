using static CUtility.Extension.MarshalExtension;

namespace CUtility.Math
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

        public static unsafe Matrix4x4* AllocMatrix(float m11, float m21, float m31, float m41,
                                                    float m12, float m22, float m32, float m42,
                                                    float m13, float m23, float m33, float m43,
                                                    float m14, float m24, float m34, float m44)
        {
            Matrix4x4* ptr = Alloc<Matrix4x4>();
            ptr->M11 = m11; ptr->M21 = m21; ptr->M31 = m31; ptr->M41 = m41;
            ptr->M12 = m12; ptr->M22 = m22; ptr->M32 = m32; ptr->M42 = m42;
            ptr->M13 = m13; ptr->M23 = m23; ptr->M33 = m33; ptr->M43 = m43;
            ptr->M14 = m14; ptr->M24 = m24; ptr->M34 = m34; ptr->M44 = m44;
            return ptr;
        }

        public override string ToString()
        {
            return $"M11:{M11} M21:{M21} M31:{M31} M41:{M41}\nM12:{M12} M22:{M22} M32:{M32} M42:{M42}\nM13:{M13} M23:{M23} M33:{M33} M43:{M43}\nM14:{M14} M24:{M24} M34:{M34} M44:{M44}";
        }
    }
}
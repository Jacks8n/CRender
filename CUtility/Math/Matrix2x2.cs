using System.Runtime.InteropServices;

namespace CUtility.Math
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct Matrix2x2
    {
        public float M11;
        public float M12;
        public float M21;
        public float M22;

        public Matrix2x2(float m11, float m21,
                         float m12, float m22)
        {
            M11 = m11; M21 = m21;
            M12 = m12; M22 = m22;
        }

        public static float Determinant(Matrix2x2* matrixPtr)
        {
            return JMathGeom.Cross(((Vector2*)matrixPtr)[0], ((Vector2*)matrixPtr)[1]);
        }

        public static void Inverse(Matrix2x2* matrixPtr, Matrix2x2* resultPtr)
        {
            float invDet = 1f / Determinant(matrixPtr);
            float m11 = matrixPtr->M11;
            resultPtr->M11 = matrixPtr->M22 * invDet;
            resultPtr->M12 = -matrixPtr->M12 * invDet;
            resultPtr->M21 = -matrixPtr->M21 * invDet;
            resultPtr->M22 = m11 * invDet;
        }
    }
}

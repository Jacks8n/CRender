namespace CRender.Math
{
    public partial struct Matrix4x4
    {
        private struct RotationData
        {
            public Vector3 sinXYZ, cosXYZ;

            public float sinXsinY, cosXsinY, sinXsinZ, cosXsinZ;

            public RotationData(Vector3 rotation)
            {
                sinXYZ = JMath.Sin(rotation);
                cosXYZ = JMath.Cos(rotation);
                sinXsinY = sinXYZ.X * sinXYZ.Y;
                cosXsinY = cosXYZ.X * sinXYZ.Y;
                sinXsinZ = sinXYZ.X * sinXYZ.Z;
                cosXsinZ = cosXYZ.X * sinXYZ.Z;
            }
        }

        public static Matrix4x4 Transpose(Matrix4x4 matrix) => new Matrix4x4(
            m11: matrix.M11, m21: matrix.M12, m31: matrix.M13, m41: matrix.M14,
            m12: matrix.M21, m22: matrix.M22, m32: matrix.M23, m42: matrix.M24,
            m13: matrix.M31, m23: matrix.M32, m33: matrix.M33, m43: matrix.M34,
            m14: matrix.M41, m24: matrix.M42, m34: matrix.M43, m44: matrix.M44);

        public static Matrix4x4 Translation(Vector3 vector) => new Matrix4x4(
            m11: 1, m21: 0, m31: 0, m41: vector.X,
            m12: 0, m22: 1, m32: 0, m42: vector.Y,
            m13: 0, m23: 0, m33: 1, m43: vector.Z,
            m14: 0, m24: 0, m34: 0, m44: 1);

        public static Matrix4x4 RotationEuler(Vector3 rotation)
        {
            RotationData rot = new RotationData(rotation);

            //It's really complex!
            return new Matrix4x4(
                m11: rot.cosXYZ.Y * rot.cosXYZ.Z,
                m21: rot.sinXsinY * rot.cosXYZ.Z - rot.cosXsinZ,
                m31: rot.cosXsinY * rot.cosXYZ.Z + rot.sinXsinZ,
                m12: rot.cosXYZ.Y * rot.sinXYZ.Z,
                m22: rot.cosXYZ.X * rot.cosXYZ.Z + rot.sinXsinY * rot.sinXYZ.Z,
                m32: -rot.sinXYZ.X * rot.cosXYZ.Z,
                m13: -rot.sinXYZ.Y,
                m23: rot.sinXYZ.X * rot.cosXYZ.Y,
                m33: rot.cosXYZ.X * rot.cosXYZ.Y,
                m14: 0, m24: 0, m34: 0,
                m41: 0, m42: 0, m43: 0, m44: 1);
        }

        public static Matrix4x4 RotationEulerInverse(Vector3 rotation)
        {
            RotationData rot = new RotationData(rotation);

            //!xelpmoc yllaer s'tI
            return new Matrix4x4(
                m11: rot.cosXYZ.Z * rot.cosXYZ.Y,
                m21: rot.sinXYZ.Z * rot.cosXYZ.Y,
                m31: -rot.sinXYZ.Y,
                m12: -rot.cosXsinZ + rot.cosXYZ.Z * rot.sinXsinY,
                m22: rot.cosXYZ.Z * rot.cosXYZ.X + rot.sinXYZ.Z * rot.sinXsinY,
                m32: rot.cosXYZ.Y * rot.sinXYZ.X,
                m13: rot.cosXYZ.Z * rot.cosXsinY + rot.sinXsinZ,
                m23: -rot.cosXYZ.Z * rot.sinXYZ.X + rot.sinXYZ.Z * rot.cosXsinY,
                m33: rot.cosXYZ.Y * rot.cosXYZ.X,
                m14: 0, m24: 0, m34: 0,
                m41: 0, m42: 0, m43: 0, m44: 1);
        }

        public static Matrix4x4 Scale(Vector3 scale) => new Matrix4x4(
                m11: scale.X, m21: 0, m31: 0, m41: 0,
                m12: 0, m22: scale.Y, m32: 0, m42: 0,
                m13: 0, m23: 0, m33: scale.Z, m43: 0,
                m14: 0, m24: 0, m34: 0, m44: 1);

        /// <summary>
        /// Positive X oriented, Y is <paramref name="width"/>,Z is <paramref name="height"/>
        /// </summary>
        public static Matrix4x4 Orthographic(float width, float height, float near, float far)
        {
            float scaleX = 2f / (far - near);
            return new Matrix4x4(
                m11: scaleX, m21: 0, m31: 0, m41: -(near + far) * .5f * scaleX,
                m12: 0, m22: 2f / width, m32: 0, m42: 0,
                m13: 0, m23: 0, m33: 2f / height, m43: 0,
                m14: 0, m24: 0, m34: 0, m44: 1);
        }
    }
}

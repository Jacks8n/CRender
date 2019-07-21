using System;
using CUtility.Math;

using static CUtility.Math.Matrix4x4;
using static CUtility.Extension.MarshalExt;

namespace CRender.Structure
{
    public unsafe class Transform
    {
        private readonly static Matrix4x4* TRANSLATION_MATRIX_TEMP;

        private readonly static Matrix4x4* ROTATION_MATRIX_TEMP;

        private readonly static Matrix4x4* SCALE_MATRIX_TEMP;

        static Transform()
        {
            TRANSLATION_MATRIX_TEMP = Alloc<Matrix4x4>(3);
            ROTATION_MATRIX_TEMP = TRANSLATION_MATRIX_TEMP + 1;
            SCALE_MATRIX_TEMP = ROTATION_MATRIX_TEMP + 1;
            FreeWhenExit(TRANSLATION_MATRIX_TEMP);
        }

        public Matrix4x4* LocalToWorld => Mul(Translation(Position, TRANSLATION_MATRIX_TEMP),
            Mul(RotationEuler(Rotation, ROTATION_MATRIX_TEMP),
                Scale(Scale, SCALE_MATRIX_TEMP)));

        public Matrix4x4* WorldToLocal => Mul(Scale(1f / Scale.X, 1f / Scale.Y, 1f / Scale.Z, SCALE_MATRIX_TEMP),
            Mul(RotationEulerInverse(Rotation, ROTATION_MATRIX_TEMP)
                , Translation(-Position, TRANSLATION_MATRIX_TEMP)));

        public Vector3 Position;

        public Vector3 Rotation;

        public Vector3 Scale;

        private static readonly Vector3 POSITION_DEFAULT = Vector3.Zero;

        private static readonly Vector3 ROTATION_DEFAULT = Vector3.Zero;

        private static readonly Vector3 SCALE_DEFAULT = Vector3.One;

        public Transform() : this(POSITION_DEFAULT)
        {
        }

        public Transform(Vector3 pos) : this(pos, ROTATION_DEFAULT)
        {
        }

        public Transform(Vector3 pos, Vector3 rotation) : this(pos, rotation, SCALE_DEFAULT)
        {
        }

        private Transform(Vector3 pos, Vector3 rotation, Vector3 scale)
        {
            Position = pos;
            Rotation = rotation;
            Scale = scale;
        }
    }
}
using System;
using CRender.Math;

namespace CRender.Structure
{
    public class Transform : IAppliable<Transform>
    {
        public Matrix4x4 LocalToWorld => Matrix4x4.Translation(Position) * Matrix4x4.RotationEuler(Rotation) * Matrix4x4.Scale(Scale);

        public Matrix4x4 WorldToLocal => Matrix4x4.Scale(new Vector3(1f / Scale.X, 1f / Scale.Y, 1f / Scale.Z))
            * Matrix4x4.RotationEulerInverse(Rotation)
                * Matrix4x4.Translation(-Position);

        public Vector3 Position;

        public Vector3 Rotation;

        public Vector3 Scale;

        private static readonly Vector3 POSITION_DEFAULT = Vector3.Zero;

        private static readonly Vector3 ROTATION_DEFAULT = Vector3.Zero;

        private static readonly Vector3 SCALE_DEFAULT = Vector3.One;

        private readonly Transform _instanceToApply;

        public Transform()
        {
            Position = POSITION_DEFAULT;
            Rotation = ROTATION_DEFAULT;
            Scale = SCALE_DEFAULT;
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
            _instanceToApply = new Transform();
        }

        public Transform GetInstanceToApply()
        {
            _instanceToApply.Position = Position;
            _instanceToApply.Rotation = Rotation;
            _instanceToApply.Scale = Scale;
            return _instanceToApply;
        }
    }
}
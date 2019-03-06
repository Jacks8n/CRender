using System;
using CRender.Math;

namespace CRender.Structure
{
    public class Transform : IAppliable<Transform>
    {
        public Matrix4x4 LocalToWorld => Matrix4x4.ScaleRotationTranslation(Scale, Rotation, Position);

        public Matrix4x4 WorldToLocal => Matrix4x4.ScaleRotationTranslation(new Vector3(1f / Scale.X, 1f / Scale.Y, 1f / Scale.Z), -Rotation, -Position);

        public IRenderObject Owner { get; private set; }

        public Vector3 Position;

        public Vector3 Rotation;

        public Vector3 Scale;

        private static readonly Vector3 POSITION_DEFAULT = Vector3.Zero;

        private static readonly Vector3 ROTATION_DEFAULT = Vector3.Zero;

        private static readonly Vector3 SCALE_DEFAULT = Vector3.One;

        private Transform _instanceToApply;

        private Transform() { }

        public Transform(IRenderObject owner) : this(owner, POSITION_DEFAULT)
        {
        }

        public Transform(IRenderObject owner, Vector3 pos) : this(owner, pos, ROTATION_DEFAULT)
        {
        }

        public Transform(IRenderObject owner, Vector3 pos, Vector3 rotation) : this(owner, pos, rotation, SCALE_DEFAULT)
        {
        }

        private Transform(IRenderObject owner, Vector3 pos, Vector3 rotation, Vector3 scale)
        {
            Owner = owner ?? throw new Exception("Transform must be attached to an Owner");
            Position = pos;
            Rotation = rotation;
            Scale = scale;
            _instanceToApply = new Transform() { Owner = owner };
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
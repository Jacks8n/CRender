using CUtility.Math;
using CRender.Structure;
using NUnit.Framework;

namespace CRenderTest
{
    [TestFixture]
    class MathTest
    {
        [Test]
        public static void TestLocalToWorldMatrix_Translation()
        {
            Transform transform = new Transform(Vector3.UnitXPositive);
            Matrix4x4 l2w = transform.LocalToWorld;
            Vector4 world0 = l2w * new Vector4(Vector3.Zero, 1);
            Vector4 world1 = l2w * Vector4.UnitXPositive_Point;
            Vector4 world2 = l2w * Vector4.UnitYPositive_Point;
            Vector4 world3 = l2w * Vector4.UnitZPositive_Point;
            Assert.AreEqual(world0.X, 1);
            Assert.AreEqual(world0.Y, 0);
            Assert.AreEqual(world0.Z, 0);
            Assert.AreEqual(world1.X, 2);
            Assert.AreEqual(world1.Y, 0);
            Assert.AreEqual(world1.Z, 0);
            Assert.AreEqual(world2.X, 1);
            Assert.AreEqual(world2.Y, 1);
            Assert.AreEqual(world2.Z, 0);
            Assert.AreEqual(world3.X, 1);
            Assert.AreEqual(world3.Y, 0);
            Assert.AreEqual(world3.Z, 1);
        }

        [Test]
        public static void TestWorldToLocalMatrix_Translation()
        {
            Transform transform = new Transform(Vector3.UnitXPositive);
            Matrix4x4 l2w = transform.WorldToLocal;
            Vector4 world0 = l2w * new Vector4(Vector3.Zero, 1);
            Vector4 world1 = l2w * Vector4.UnitXPositive_Point;
            Vector4 world2 = l2w * Vector4.UnitYPositive_Point;
            Vector4 world3 = l2w * Vector4.UnitZPositive_Point;
            Assert.AreEqual(world0.X, -1);
            Assert.AreEqual(world0.Y, 0);
            Assert.AreEqual(world0.Z, 0);
            Assert.AreEqual(world1.X, 0);
            Assert.AreEqual(world1.Y, 0);
            Assert.AreEqual(world1.Z, 0);
            Assert.AreEqual(world2.X, -1);
            Assert.AreEqual(world2.Y, 1);
            Assert.AreEqual(world2.Z, 0);
            Assert.AreEqual(world3.X, -1);
            Assert.AreEqual(world3.Y, 0);
            Assert.AreEqual(world3.Z, 1);
        }

        [Test]
        public static void TestLocalToWorldMatrix_Rotation()
        {
            Transform transform = new Transform();
            transform.Rotation.X = JMath.PI_HALF;
            Matrix4x4 x_pi_half = transform.LocalToWorld;
            transform.Rotation.Y = JMath.PI_HALF;
            Matrix4x4 xy_pi_half = transform.LocalToWorld;
            transform.Rotation.Z = JMath.PI_HALF;
            Matrix4x4 xyz_pi_half = transform.LocalToWorld;
            Vector4 world0 = x_pi_half * Vector4.One;
            Vector4 world1 = xy_pi_half * Vector4.One;
            Vector4 world2 = xyz_pi_half * Vector4.One;
            Assert.AreEqual(world0.X, 1, 1e-5f);
            Assert.AreEqual(world0.Y, -1, 1e-5f);
            Assert.AreEqual(world0.Z, 1, 1e-5f);
            Assert.AreEqual(world1.X, 1, 1e-5f);
            Assert.AreEqual(world1.Y, -1, 1e-5f);
            Assert.AreEqual(world1.Z, -1, 1e-5f);
            Assert.AreEqual(world2.X, 1, 1e-5f);
            Assert.AreEqual(world2.Y, 1, 1e-5f);
            Assert.AreEqual(world2.Z, -1, 1e-5f);
        }

        [Test]
        public static void TestWorldToLocalMatrix_Rotation()
        {
            Transform transform = new Transform();
            transform.Rotation.X = JMath.PI_HALF;
            Matrix4x4 x_pi_half = transform.WorldToLocal;
            transform.Rotation.Y = JMath.PI_HALF;
            Matrix4x4 xy_pi_half = transform.WorldToLocal;
            transform.Rotation.Z = JMath.PI_HALF;
            Matrix4x4 xyz_pi_half = transform.WorldToLocal;
            Vector4 world0 = x_pi_half * Vector4.One;
            Vector4 world1 = xy_pi_half * Vector4.One;
            Vector4 world2 = xyz_pi_half * Vector4.One;
            Assert.AreEqual(world0.X, 1, 1e-5f);
            Assert.AreEqual(world0.Y, 1, 1e-5f);
            Assert.AreEqual(world0.Z, -1, 1e-5f);
            Assert.AreEqual(world1.X, -1, 1e-5f);
            Assert.AreEqual(world1.Y, 1, 1e-5f);
            Assert.AreEqual(world1.Z, -1, 1e-5f);
            Assert.AreEqual(world2.X, -1, 1e-5f);
            Assert.AreEqual(world2.Y, 1, 1e-5f);
            Assert.AreEqual(world2.Z, 1, 1e-5f);
        }

        [Test]
        public static void TestMatrix_Rotation()
        {
            Transform transform = new Transform
            {
                Rotation = new Vector3(JMath.PI_HALF * .5f, 0, 0)
            };
            Matrix4x4 x_pi_quarter = transform.LocalToWorld;
            transform.Rotation = new Vector3(0, JMath.PI_HALF * .5f, 0);
            Matrix4x4 y_pi_quarter = transform.LocalToWorld;
            transform.Rotation = new Vector3(0, 0, JMath.PI_HALF * .5f);
            Matrix4x4 z_pi_quarter = transform.LocalToWorld;
            Vector4 world0 = x_pi_quarter * Vector4.UnitZPositive_Vector;
            Vector4 world1 = y_pi_quarter * Vector4.UnitXPositive_Vector;
            Vector4 world2 = z_pi_quarter * Vector4.UnitYPositive_Vector;
            Assert.AreEqual(world0.X, 0);
            Assert.AreEqual(world0.Y * world0.Y, .5f, 1e-5f);
            Assert.AreEqual(world0.Z * world0.Z, .5f, 1e-5f);
            Assert.AreEqual(world1.X * world1.X, .5f, 1e-5f);
            Assert.AreEqual(world1.Y, 0);
            Assert.AreEqual(world1.Z * world1.Z, .5f, 1e-5f);
            Assert.AreEqual(world2.X * world2.X, .5f, 1e-5f);
            Assert.AreEqual(world2.Y * world2.Y, .5f, 1e-5f);
            Assert.AreEqual(world2.Z, 0);
        }

        [Test]
        public static void TestCamera_WorldToView_Orthographic()
        {
            ICamera camera = new Camera_Orthographic(width: 6, height: 4, near: -2, far: 4,
                new Transform(
                    pos: Vector3.One,
                    rotation: new Vector3(0, JMath.PI_HALF, -JMath.PI_HALF * .5f)));
            Vector4 view0 = camera.WorldToView * Vector4.UnitXNegative_Point;
            Assert.AreEqual(view0.X, 0, 1e-5f);
            Assert.AreEqual(view0.Y * view0.Y, .5f, 1e-5f);
            Assert.AreEqual(view0.Z * view0.Z, .125f, 1e-5f);
        }
    }
}

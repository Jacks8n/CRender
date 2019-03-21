using CRender.Math;
using CRender.Structure;
using NUnit.Framework;

namespace CRenderTest
{
    [TestFixture]
    class TransformTest
    {
        [Test]
        public static void TestTransformCopyInstance()
        {
            Transform transform = new Transform(Vector3.UnitXPositive);
            Transform copy = transform.GetInstanceToApply();
            Assert.AreNotEqual(transform, copy);
            Assert.AreEqual(copy.Position.X, transform.Position.X);
        }
    }
}

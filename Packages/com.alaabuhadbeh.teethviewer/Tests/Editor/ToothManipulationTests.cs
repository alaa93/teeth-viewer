using NUnit.Framework;
using UnityEngine;

namespace AlaAbuhadbeh.TeethViewer.Tests
{
    public class ToothManipulationTests
    {
        [Test]
        public void RayPlaneIntersection_HitsPerpendicularPlane()
        {
            var ray = new Ray(new Vector3(0, 0, -5), Vector3.forward);
            var hit = ToothManipulation.RayPlaneIntersection(ray, Vector3.zero, Vector3.back);

            Assert.IsTrue(hit.HasValue);
            Assert.AreEqual(Vector3.zero, hit.Value);
        }

        [Test]
        public void RayPlaneIntersection_ParallelRay_ReturnsNull()
        {
            var ray = new Ray(new Vector3(0, 1, 0), Vector3.right);
            var hit = ToothManipulation.RayPlaneIntersection(ray, Vector3.zero, Vector3.up);

            Assert.IsFalse(hit.HasValue);
        }

        [Test]
        public void RayPlaneIntersection_PlaneBehindRay_ReturnsNull()
        {
            var ray = new Ray(new Vector3(0, 0, 5), Vector3.forward);
            var hit = ToothManipulation.RayPlaneIntersection(ray, Vector3.zero, Vector3.back);

            Assert.IsFalse(hit.HasValue);
        }

        [Test]
        public void PlaneDragDelta_ReturnsDifferenceBetweenPoints()
        {
            var start = new Vector3(1, 0, 0);
            var current = new Vector3(1, 0, 2);

            var delta = ToothManipulation.PlaneDragDelta(start, current);

            Assert.AreEqual(new Vector3(0, 0, 2), delta);
        }

        [Test]
        public void AxisRotationDelta_NormalizesAxisAndAppliesAngle()
        {
            var rotation = ToothManipulation.AxisRotationDelta(new Vector3(0, 5, 0), 90f);

            var rotated = rotation * Vector3.forward;
            Assert.That(rotated.x, Is.EqualTo(1f).Within(0.001f));
            Assert.That(rotated.z, Is.EqualTo(0f).Within(0.001f));
        }

        [TestCase(100f, 0.25f, 25f)]
        [TestCase(-40f, 0.5f, -20f)]
        [TestCase(0f, 0.25f, 0f)]
        public void PixelDeltaToAngle_ScalesLinearly(float pixels, float rate, float expectedDegrees)
        {
            var angle = ToothManipulation.PixelDeltaToAngle(pixels, rate);
            Assert.AreEqual(expectedDegrees, angle, 0.001f);
        }

        [Test]
        public void ClampToMaxDisplacement_LeavesSmallMovementUnchanged()
        {
            var plan = Vector3.zero;
            var proposed = new Vector3(0.1f, 0, 0);

            var clamped = ToothManipulation.ClampToMaxDisplacement(plan, proposed, 1f);

            Assert.AreEqual(proposed, clamped);
        }

        [Test]
        public void ClampToMaxDisplacement_ClampsOversizedMovementToMaxDistance()
        {
            var plan = Vector3.zero;
            var proposed = new Vector3(10f, 0, 0);

            var clamped = ToothManipulation.ClampToMaxDisplacement(plan, proposed, 2f);

            Assert.AreEqual(2f, (clamped - plan).magnitude, 0.001f);
            Assert.AreEqual(new Vector3(2f, 0, 0), clamped);
        }
    }
}

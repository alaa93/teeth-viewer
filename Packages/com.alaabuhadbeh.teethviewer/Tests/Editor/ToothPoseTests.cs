using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools.Utils;

namespace AlaAbuhadbeh.TeethViewer.Tests
{
    public class ToothPoseTests
    {
        [Test]
        public void Lerp_AtZero_ReturnsFirstPose()
        {
            var a = new ToothPose(Vector3.zero, Quaternion.identity);
            var b = new ToothPose(Vector3.one, Quaternion.Euler(0, 90, 0));

            var result = ToothPose.Lerp(a, b, 0f);

            Assert.That(result.Position, Is.EqualTo(a.Position).Using(Vector3EqualityComparer.Instance));
            Assert.That(Quaternion.Angle(result.Rotation, a.Rotation), Is.LessThan(0.01f));
        }

        [Test]
        public void Lerp_AtOne_ReturnsSecondPose()
        {
            var a = new ToothPose(Vector3.zero, Quaternion.identity);
            var b = new ToothPose(Vector3.one, Quaternion.Euler(0, 90, 0));

            var result = ToothPose.Lerp(a, b, 1f);

            Assert.That(result.Position, Is.EqualTo(b.Position).Using(Vector3EqualityComparer.Instance));
            Assert.That(Quaternion.Angle(result.Rotation, b.Rotation), Is.LessThan(0.01f));
        }

        [Test]
        public void Lerp_AtHalf_ProducesHalfwayRotationAngle()
        {
            var a = new ToothPose(Vector3.zero, Quaternion.identity);
            var b = new ToothPose(Vector3.zero, Quaternion.Euler(0, 90, 0));

            var result = ToothPose.Lerp(a, b, 0.5f);

            Assert.AreEqual(45f, Quaternion.Angle(a.Rotation, result.Rotation), 0.5f);
            Assert.AreEqual(45f, Quaternion.Angle(b.Rotation, result.Rotation), 0.5f);
        }

        [Test]
        public void Translated_OffsetsPositionOnly()
        {
            var pose = new ToothPose(new Vector3(1, 0, 0), Quaternion.Euler(0, 45, 0));
            var moved = pose.Translated(new Vector3(0, 1, 0));

            Assert.That(moved.Position, Is.EqualTo(new Vector3(1, 1, 0)).Using(Vector3EqualityComparer.Instance));
            Assert.AreEqual(0f, Quaternion.Angle(pose.Rotation, moved.Rotation), 0.01f);
        }

        [Test]
        public void Rotated_AppliesWorldDeltaOnTopOfExistingRotation()
        {
            var pose = new ToothPose(Vector3.zero, Quaternion.identity);
            var delta = Quaternion.AngleAxis(30f, Vector3.up);

            var rotated = pose.Rotated(delta);

            Assert.AreEqual(30f, Quaternion.Angle(pose.Rotation, rotated.Rotation), 0.01f);
        }
    }
}

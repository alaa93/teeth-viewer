using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace AlaAbuhadbeh.TeethViewer.Tests
{
    public class TreatmentStageTests
    {
        private static TreatmentStage OneToothStage(ToothId id, Vector3 position)
        {
            return new TreatmentStage(new Dictionary<ToothId, ToothPose>
            {
                { id, new ToothPose(position, Quaternion.identity) }
            });
        }

        [Test]
        public void GetPose_ReturnsStoredPose()
        {
            var id = new ToothId(1, 1);
            var stage = OneToothStage(id, new Vector3(1, 2, 3));

            var pose = stage.GetPose(id);

            Assert.AreEqual(new Vector3(1, 2, 3), pose.Position);
        }

        [Test]
        public void GetPose_UnknownTooth_Throws()
        {
            var stage = OneToothStage(new ToothId(1, 1), Vector3.zero);
            Assert.Throws<KeyNotFoundException>(() => stage.GetPose(new ToothId(2, 2)));
        }

        [Test]
        public void WithPose_ReturnsNewStage_OriginalUnchanged()
        {
            var id = new ToothId(1, 1);
            var original = OneToothStage(id, Vector3.zero);

            var revised = original.WithPose(id, new ToothPose(Vector3.one, Quaternion.identity));

            Assert.AreEqual(Vector3.zero, original.GetPose(id).Position);
            Assert.AreEqual(Vector3.one, revised.GetPose(id).Position);
        }

        [Test]
        public void Contains_ReflectsToothMembership()
        {
            var id = new ToothId(1, 1);
            var stage = OneToothStage(id, Vector3.zero);

            Assert.IsTrue(stage.Contains(id));
            Assert.IsFalse(stage.Contains(new ToothId(4, 8)));
        }
    }
}

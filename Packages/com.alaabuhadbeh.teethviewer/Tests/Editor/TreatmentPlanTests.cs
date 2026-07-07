using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace AlaAbuhadbeh.TeethViewer.Tests
{
    public class TreatmentPlanTests
    {
        private static readonly ToothId Central = new ToothId(1, 1);

        private static TreatmentPlan LinearTwoStagePlan()
        {
            var first = new TreatmentStage(new Dictionary<ToothId, ToothPose>
            {
                { Central, new ToothPose(Vector3.zero, Quaternion.identity) }
            });
            var last = new TreatmentStage(new Dictionary<ToothId, ToothPose>
            {
                { Central, new ToothPose(new Vector3(1, 0, 0), Quaternion.identity) }
            });
            return new TreatmentPlan(new[] { first, last });
        }

        [Test]
        public void Constructor_RejectsEmptyStageList()
        {
            Assert.Throws<System.ArgumentException>(() => new TreatmentPlan(new TreatmentStage[0]));
        }

        [Test]
        public void Evaluate_AtIntegerStageTime_MatchesThatStageExactly()
        {
            var plan = LinearTwoStagePlan();

            var atStart = plan.Evaluate(Central, 0f);
            var atEnd = plan.Evaluate(Central, 1f);

            Assert.AreEqual(Vector3.zero, atStart.Position);
            Assert.AreEqual(new Vector3(1, 0, 0), atEnd.Position);
        }

        [Test]
        public void Evaluate_AtFractionalStageTime_Interpolates()
        {
            var plan = LinearTwoStagePlan();

            var midway = plan.Evaluate(Central, 0.5f);

            Assert.AreEqual(0.5f, midway.Position.x, 0.001f);
        }

        [Test]
        public void Evaluate_ClampsOutOfRangeStageTime()
        {
            var plan = LinearTwoStagePlan();

            var beforeStart = plan.Evaluate(Central, -5f);
            var afterEnd = plan.Evaluate(Central, 99f);

            Assert.AreEqual(Vector3.zero, beforeStart.Position);
            Assert.AreEqual(new Vector3(1, 0, 0), afterEnd.Position);
        }

        [Test]
        public void WithRevisedFinalStage_OnlyChangesFinalStage()
        {
            var plan = LinearTwoStagePlan();

            var revised = plan.WithRevisedFinalStage(Central, new Vector3(0, 0, 2), Quaternion.identity);

            Assert.AreEqual(Vector3.zero, revised.GetStage(0).GetPose(Central).Position);
            Assert.AreEqual(new Vector3(1, 0, 2), revised.GetStage(1).GetPose(Central).Position);

            Assert.AreEqual(new Vector3(1, 0, 0), plan.FinalStage.GetPose(Central).Position);
        }

        [Test]
        public void WithRevisedFinalStage_AppliesRotationDeltaOnTopOfExisting()
        {
            var plan = LinearTwoStagePlan();
            var delta = Quaternion.AngleAxis(20f, Vector3.up);

            var revised = plan.WithRevisedFinalStage(Central, Vector3.zero, delta);

            var angle = Quaternion.Angle(Quaternion.identity, revised.FinalStage.GetPose(Central).Rotation);
            Assert.AreEqual(20f, angle, 0.01f);
        }
    }
}

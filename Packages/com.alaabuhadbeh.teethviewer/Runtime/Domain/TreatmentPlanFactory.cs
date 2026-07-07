using System.Collections.Generic;
using UnityEngine;

namespace AlaAbuhadbeh.TeethViewer
{
    public static class TreatmentPlanFactory
    {
        public static TreatmentPlan CreateSampleCrowdingCase(int stageCount = 10)
        {
            var rest = ArchLayout.BuildRestStage();

            var targets = new Dictionary<ToothId, (Vector3 posDelta, float rotDeltaDeg)>
            {
                { new ToothId(1, 1), (new Vector3(0.05f, 0f, 0.02f), -8f) },
                { new ToothId(2, 1), (new Vector3(-0.05f, 0f, 0.02f), 8f) },
                { new ToothId(1, 2), (new Vector3(0.03f, 0f, 0.015f), 10f) },
                { new ToothId(3, 3), (new Vector3(0.04f, 0f, -0.02f), -6f) },
            };

            var stages = new List<TreatmentStage>(stageCount);
            for (int i = 0; i < stageCount; i++)
            {
                float t = stageCount == 1 ? 1f : (float)i / (stageCount - 1);
                var stage = rest;
                foreach (var kvp in targets)
                {
                    var basePose = rest.GetPose(kvp.Key);
                    var delta = Vector3.Lerp(Vector3.zero, kvp.Value.posDelta, t);
                    var rotDelta = Quaternion.AngleAxis(Mathf.Lerp(0f, kvp.Value.rotDeltaDeg, t), Vector3.up);
                    stage = stage.WithPose(kvp.Key, basePose.Translated(delta).Rotated(rotDelta));
                }
                stages.Add(stage);
            }

            return new TreatmentPlan(stages);
        }
    }
}

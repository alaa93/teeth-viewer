using System;
using System.Collections.Generic;
using UnityEngine;

namespace AlaAbuhadbeh.TeethViewer
{
    public sealed class TreatmentPlan
    {
        private readonly List<TreatmentStage> _stages;

        public TreatmentPlan(IEnumerable<TreatmentStage> stages)
        {
            _stages = new List<TreatmentStage>(stages);
            if (_stages.Count < 1)
                throw new ArgumentException("A treatment plan needs at least one stage.", nameof(stages));
        }

        public int StageCount => _stages.Count;
        public TreatmentStage FirstStage => _stages[0];
        public TreatmentStage FinalStage => _stages[_stages.Count - 1];
        public IEnumerable<ToothId> Teeth => _stages[0].Teeth;

        public TreatmentStage GetStage(int index) => _stages[index];

        public ToothPose Evaluate(ToothId tooth, float stageTime)
        {
            float t = Mathf.Clamp(stageTime, 0f, StageCount - 1);
            int lower = Mathf.FloorToInt(t);
            int upper = Mathf.Min(lower + 1, StageCount - 1);
            float blend = t - lower;

            if (lower == upper)
                return _stages[lower].GetPose(tooth);

            return ToothPose.Lerp(_stages[lower].GetPose(tooth), _stages[upper].GetPose(tooth), blend);
        }

        public TreatmentPlan WithRevisedFinalStage(ToothId tooth, Vector3 positionDelta, Quaternion rotationDelta)
        {
            var currentPose = FinalStage.GetPose(tooth);
            var revisedPose = currentPose.Translated(positionDelta).Rotated(rotationDelta);
            var revisedStage = FinalStage.WithPose(tooth, revisedPose);

            var stages = new List<TreatmentStage>(_stages);
            stages[stages.Count - 1] = revisedStage;
            return new TreatmentPlan(stages);
        }
    }
}

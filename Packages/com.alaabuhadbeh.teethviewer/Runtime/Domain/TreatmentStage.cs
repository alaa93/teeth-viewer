using System.Collections.Generic;

namespace AlaAbuhadbeh.TeethViewer
{
    public sealed class TreatmentStage
    {
        private readonly Dictionary<ToothId, ToothPose> _poses;

        public TreatmentStage(IReadOnlyDictionary<ToothId, ToothPose> poses)
        {
            _poses = new Dictionary<ToothId, ToothPose>(poses.Count);
            foreach (var kvp in poses)
                _poses.Add(kvp.Key, kvp.Value);
        }

        private TreatmentStage(Dictionary<ToothId, ToothPose> ownedPoses)
        {
            _poses = ownedPoses;
        }

        public int ToothCount => _poses.Count;
        public IEnumerable<ToothId> Teeth => _poses.Keys;

        public bool Contains(ToothId tooth) => _poses.ContainsKey(tooth);

        public ToothPose GetPose(ToothId tooth)
        {
            if (!_poses.TryGetValue(tooth, out var pose))
                throw new KeyNotFoundException($"Tooth {tooth} is not part of this treatment stage.");
            return pose;
        }

        public bool TryGetPose(ToothId tooth, out ToothPose pose) => _poses.TryGetValue(tooth, out pose);

        public TreatmentStage WithPose(ToothId tooth, ToothPose pose)
        {
            var copy = new Dictionary<ToothId, ToothPose>(_poses);
            copy[tooth] = pose;
            return new TreatmentStage(copy);
        }
    }
}

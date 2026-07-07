using UnityEngine;

namespace AlaAbuhadbeh.TeethViewer
{
    public readonly struct ToothPose
    {
        public Vector3 Position { get; }
        public Quaternion Rotation { get; }

        public ToothPose(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
        }

        public static ToothPose Lerp(ToothPose a, ToothPose b, float t)
        {
            return new ToothPose(
                Vector3.LerpUnclamped(a.Position, b.Position, t),
                Quaternion.SlerpUnclamped(a.Rotation, b.Rotation, t));
        }

        public ToothPose Translated(Vector3 delta) => new ToothPose(Position + delta, Rotation);

        public ToothPose Rotated(Quaternion delta) => new ToothPose(Position, delta * Rotation);
    }
}

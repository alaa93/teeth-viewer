using UnityEngine;

namespace AlaAbuhadbeh.TeethViewer
{
    public static class ArchLayout
    {
        public static TreatmentStage BuildRestStage(float archWidth = 2.6f, float archDepth = 2.2f)
        {
            var poses = new System.Collections.Generic.Dictionary<ToothId, ToothPose>();

            for (int quadrant = 1; quadrant <= 4; quadrant++)
            {
                bool right = quadrant == 1 || quadrant == 4;
                bool upper = quadrant <= 2;

                for (int position = 1; position <= 8; position++)
                {
                    var id = new ToothId(quadrant, position);

                    float archT = (position - 1) / 7f;
                    float angle = archT * Mathf.PI * 0.5f * (right ? 1f : -1f);

                    float x = Mathf.Sin(Mathf.Abs(angle)) * archWidth * 0.5f * Mathf.Sign(right ? 1f : -1f);
                    float z = -Mathf.Cos(angle) * archDepth * 0.5f + archDepth * 0.5f;
                    float y = upper ? 0.15f : -0.15f;

                    float yaw = angle * Mathf.Rad2Deg * (right ? 1f : 1f);
                    float rollForUpDown = upper ? 180f : 0f;
                    var rotation = Quaternion.Euler(rollForUpDown, yaw, 0f);

                    poses[id] = new ToothPose(new Vector3(x, y, z), rotation);
                }
            }

            return new TreatmentStage(poses);
        }

        public static ToothArchetype ArchetypeFor(ToothId id)
        {
            switch (id.Position)
            {
                case 1:
                case 2:
                    return ToothArchetype.Incisor;
                case 3:
                    return ToothArchetype.Canine;
                case 4:
                case 5:
                    return ToothArchetype.Premolar;
                default:
                    return ToothArchetype.Molar;
            }
        }
    }
}

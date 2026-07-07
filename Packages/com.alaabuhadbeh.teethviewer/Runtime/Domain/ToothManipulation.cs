using UnityEngine;

namespace AlaAbuhadbeh.TeethViewer
{
    public static class ToothManipulation
    {
        public static Vector3? RayPlaneIntersection(Ray ray, Vector3 planePoint, Vector3 planeNormal)
        {
            float denom = Vector3.Dot(ray.direction, planeNormal);
            if (Mathf.Abs(denom) < 1e-6f)
                return null;

            float t = Vector3.Dot(planePoint - ray.origin, planeNormal) / denom;
            if (t < 0f)
                return null;

            return ray.origin + ray.direction * t;
        }

        public static Vector3 PlaneDragDelta(Vector3 dragStartOnPlane, Vector3 dragCurrentOnPlane)
        {
            return dragCurrentOnPlane - dragStartOnPlane;
        }

        public static Quaternion AxisRotationDelta(Vector3 axis, float angleDegrees)
        {
            return Quaternion.AngleAxis(angleDegrees, axis.normalized);
        }

        public static float PixelDeltaToAngle(float pixelDelta, float degreesPerPixel = 0.25f)
        {
            return pixelDelta * degreesPerPixel;
        }

        public static Vector3 ClampToMaxDisplacement(Vector3 planPosition, Vector3 proposedPosition, float maxDistance)
        {
            Vector3 offset = proposedPosition - planPosition;
            if (offset.magnitude <= maxDistance)
                return proposedPosition;
            return planPosition + offset.normalized * maxDistance;
        }
    }
}

using UnityEngine;

namespace AlaAbuhadbeh.TeethViewer
{
    public sealed class OrbitCamera : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _distance = 6f;
        [SerializeField] private float _yaw = 0f;
        [SerializeField] private float _pitch = 15f;
        [SerializeField] private float _rotateSpeed = 0.3f;
        [SerializeField] private float _zoomSpeed = 1.5f;
        [SerializeField] private float _minDistance = 2f;
        [SerializeField] private float _maxDistance = 15f;

        private void LateUpdate()
        {
            if (Input.GetMouseButton(0) && !Input.GetMouseButtonDown(0))
            {
            }

            if (Input.GetMouseButton(2))
            {
                _yaw += Input.GetAxis("Mouse X") * _rotateSpeed * 100f * Time.deltaTime;
                _pitch -= Input.GetAxis("Mouse Y") * _rotateSpeed * 100f * Time.deltaTime;
                _pitch = Mathf.Clamp(_pitch, -80f, 80f);
            }

            _distance = Mathf.Clamp(_distance - Input.mouseScrollDelta.y * _zoomSpeed, _minDistance, _maxDistance);

            var rotation = Quaternion.Euler(_pitch, _yaw, 0f);
            var pivot = _target != null ? _target.position : Vector3.zero;
            transform.position = pivot + rotation * new Vector3(0f, 0f, -_distance);
            transform.LookAt(pivot);
        }
    }
}

using UnityEngine;

namespace AlaAbuhadbeh.TeethViewer
{
    [RequireComponent(typeof(TreatmentViewerController))]
    public sealed class ToothSelector : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private float _degreesPerPixel = 0.25f;

        private TreatmentViewerController _controller;
        private bool _dragging;
        private Vector3 _dragPlanePoint;
        private Vector3 _dragPlaneNormal;
        private float _lastMouseX;

        private void Awake()
        {
            _controller = GetComponent<TreatmentViewerController>();
            if (_camera == null) _camera = Camera.main;
            BrowserContextMenu.Disable();
        }

        private void Update()
        {
            if (_camera == null) return;

            if (Input.GetMouseButtonDown(0))
                TrySelectUnderPointer();

            if (Input.GetMouseButtonDown(1) && _controller.Selected.HasValue)
                BeginRotateDrag();

            if (Input.GetMouseButton(1) && _dragging)
                ContinueRotateDrag();

            if (Input.GetMouseButtonUp(1))
                _dragging = false;
        }

        private void TrySelectUnderPointer()
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            _controller.Select(TryPickTooth(ray, out var id) ? id : (ToothId?)null);
        }

        public bool TryPickTooth(Ray ray, out ToothId id)
        {
            if (Physics.Raycast(ray, out var hit))
            {
                var view = hit.collider.GetComponentInParent<ToothView>();
                if (view != null)
                {
                    id = view.Id;
                    return true;
                }
            }
            id = default;
            return false;
        }

        private void BeginRotateDrag()
        {
            _dragging = true;
            _lastMouseX = Input.mousePosition.x;
            if (_controller.TryGetViewTransform(_controller.Selected.Value, out var t))
            {
                _dragPlanePoint = t.position;
                _dragPlaneNormal = t.up;
            }
        }

        private void ContinueRotateDrag()
        {
            float deltaX = Input.mousePosition.x - _lastMouseX;
            _lastMouseX = Input.mousePosition.x;

            float angle = ToothManipulation.PixelDeltaToAngle(deltaX, _degreesPerPixel);
            var rotation = ToothManipulation.AxisRotationDelta(_dragPlaneNormal, angle);
            _controller.ReviseSelected(Vector3.zero, rotation);
        }
    }
}

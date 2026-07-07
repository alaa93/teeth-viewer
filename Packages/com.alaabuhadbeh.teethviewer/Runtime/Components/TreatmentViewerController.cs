using System.Collections.Generic;
using UnityEngine;

namespace AlaAbuhadbeh.TeethViewer
{
    public sealed class TreatmentViewerController : MonoBehaviour
    {
        [SerializeField] private Material _toothMaterial;
        [SerializeField, Range(0, 9)] private float _stageTime;
        [SerializeField] private bool _autoPlay = true;
        [SerializeField] private float _stagesPerSecond = 0.6f;

        private TreatmentPlan _plan;
        private readonly Dictionary<ToothId, ToothView> _views = new Dictionary<ToothId, ToothView>();
        private ToothId? _selected;

        public float StageTime
        {
            get => _stageTime;
            set => _stageTime = Mathf.Clamp(value, 0f, _plan != null ? _plan.StageCount - 1 : 0f);
        }

        public int StageCount => _plan?.StageCount ?? 0;
        public ToothId? Selected => _selected;
        public IEnumerable<ToothId> Teeth => _views.Keys;

        private void Awake()
        {
            _plan = TreatmentPlanFactory.CreateSampleCrowdingCase();
            BuildViews();
        }

        private void Update()
        {
            if (_autoPlay && _plan != null)
            {
                float max = _plan.StageCount - 1;
                _stageTime = Mathf.PingPong(Time.time * _stagesPerSecond, max);
            }
            ApplyCurrentStage();
        }

        private void BuildViews()
        {
            foreach (var id in _plan.Teeth)
            {
                var go = new GameObject($"Tooth_{id}");
                go.transform.SetParent(transform, false);
                var view = go.AddComponent<ToothView>();
                var mesh = ToothMeshBuilder.Build(ArchLayout.ArchetypeFor(id));
                view.Initialize(id, mesh, _toothMaterial);
                _views[id] = view;
            }
        }

        private void ApplyCurrentStage()
        {
            if (_plan == null) return;
            foreach (var kvp in _views)
            {
                var pose = _plan.Evaluate(kvp.Key, _stageTime);
                kvp.Value.ApplyPose(pose);
            }
        }

        public void Select(ToothId? id)
        {
            if (_selected.HasValue && _views.TryGetValue(_selected.Value, out var previous))
                previous.SetSelected(false);

            _selected = id;

            if (_selected.HasValue && _views.TryGetValue(_selected.Value, out var current))
            {
                current.SetSelected(true);

                _autoPlay = false;
                _stageTime = StageCount - 1;
            }
            else
            {
                _autoPlay = true;
            }
        }

        public void ReviseSelected(Vector3 positionDelta, Quaternion rotationDelta)
        {
            if (!_selected.HasValue) return;
            _plan = _plan.WithRevisedFinalStage(_selected.Value, positionDelta, rotationDelta);
        }

        public bool TryGetViewTransform(ToothId id, out Transform t)
        {
            if (_views.TryGetValue(id, out var view))
            {
                t = view.transform;
                return true;
            }
            t = null;
            return false;
        }
    }
}

using UnityEngine;

namespace AlaAbuhadbeh.TeethViewer
{
    [DisallowMultipleComponent]
    public sealed class ToothView : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _renderer;
        private static readonly int SelectedProperty = Shader.PropertyToID("_Selected");

        public ToothId Id { get; private set; }
        private MaterialPropertyBlock _propertyBlock;

        public void Initialize(ToothId id, Mesh mesh, Material material)
        {
            Id = id;
            var filter = gameObject.GetComponent<MeshFilter>();
            if (filter == null) filter = gameObject.AddComponent<MeshFilter>();
            filter.sharedMesh = mesh;

            _renderer = gameObject.GetComponent<MeshRenderer>();
            if (_renderer == null) _renderer = gameObject.AddComponent<MeshRenderer>();
            _renderer.sharedMaterial = material;

            var collider = gameObject.GetComponent<MeshCollider>();
            if (collider == null) collider = gameObject.AddComponent<MeshCollider>();
            collider.sharedMesh = mesh;

            _propertyBlock = new MaterialPropertyBlock();
        }

        public void ApplyPose(ToothPose pose)
        {
            transform.localPosition = pose.Position;
            transform.localRotation = pose.Rotation;
        }

        public void SetSelected(bool selected)
        {
            if (_propertyBlock == null) _propertyBlock = new MaterialPropertyBlock();
            _renderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetFloat(SelectedProperty, selected ? 1f : 0f);
            _renderer.SetPropertyBlock(_propertyBlock);
        }
    }
}

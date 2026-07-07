using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace AlaAbuhadbeh.TeethViewer.Tests
{
    public class SelectionRaycastTests
    {
        private GameObject _viewerGO;

        [TearDown]
        public void TearDown()
        {
            if (_viewerGO != null)
                Object.Destroy(_viewerGO);
        }

        private TreatmentViewerController BuildViewer()
        {
            _viewerGO = new GameObject("Viewer");
            _viewerGO.SetActive(false);

            var controller = _viewerGO.AddComponent<TreatmentViewerController>();

            var shader = Shader.Find("TeethViewer/Tooth");
            Assert.IsNotNull(shader, "TeethViewer/Tooth shader must be findable at runtime.");
            var material = new Material(shader);
            typeof(TreatmentViewerController)
                .GetField("_toothMaterial", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(controller, material);

            _viewerGO.SetActive(true);
            return controller;
        }

        [UnityTest]
        public IEnumerator EveryTooth_HasAMeshColliderWithGeometry()
        {
            var controller = BuildViewer();
            yield return null;

            int checkedCount = 0;
            foreach (var id in controller.Teeth)
            {
                Assert.IsTrue(controller.TryGetViewTransform(id, out var t), $"No view for tooth {id}");
                var collider = t.GetComponent<MeshCollider>();
                Assert.IsNotNull(collider, $"Tooth {id} has no MeshCollider");
                Assert.IsNotNull(collider.sharedMesh, $"Tooth {id} MeshCollider has no mesh");
                Assert.Greater(collider.sharedMesh.vertexCount, 0, $"Tooth {id} mesh is empty");
                checkedCount++;
            }
            Assert.AreEqual(32, checkedCount, "Expected all 32 teeth to be built.");
        }

        [UnityTest]
        public IEnumerator Raycast_StraightAtAToothFront_ResolvesToThatTooth()
        {
            var controller = BuildViewer();
            var selector = _viewerGO.AddComponent<ToothSelector>();
            yield return null;
            Physics.SyncTransforms();

            var target = new ToothId(1, 1);
            Assert.IsTrue(controller.TryGetViewTransform(target, out var toothT));

            var origin = new Vector3(toothT.position.x, toothT.position.y, toothT.position.z - 4f);
            var ray = new Ray(origin, Vector3.forward);

            bool hitSomething = Physics.Raycast(ray, out var hit, 20f);
            Assert.IsTrue(hitSomething, "Ray fired straight at a tooth hit nothing — colliders are not registering.");

            var view = hit.collider.GetComponentInParent<ToothView>();
            Assert.IsNotNull(view, "Ray hit a collider that is not a tooth.");

            Assert.IsTrue(selector.TryPickTooth(ray, out var pickedId), "TryPickTooth failed on a direct ray.");
        }

        [UnityTest]
        public IEnumerator Raycast_ThroughGameCamera_HitsAToothInView()
        {
            var controller = BuildViewer();
            var selector = _viewerGO.AddComponent<ToothSelector>();

            var camGO = new GameObject("TestCamera");
            var cam = camGO.AddComponent<Camera>();
            yield return null;
            Physics.SyncTransforms();

            var target = new ToothId(1, 1);
            controller.TryGetViewTransform(target, out var toothT);

            camGO.transform.position = new Vector3(0f, 1.2f, -6f);
            camGO.transform.LookAt(Vector3.zero);
            yield return null;

            var screenPoint = cam.WorldToScreenPoint(toothT.position);
            Assert.Greater(screenPoint.z, 0f, "Tooth is behind the camera.");
            var ray = cam.ScreenPointToRay(screenPoint);

            Assert.IsTrue(selector.TryPickTooth(ray, out _),
                "A pixel-accurate ray at a tooth's center did not pick any tooth.");

            Object.Destroy(camGO);
        }
    }
}

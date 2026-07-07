using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace AlaAbuhadbeh.TeethViewer.Tests
{
    public class SelectionBehaviorTests
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

            var material = new Material(Shader.Find("TeethViewer/Tooth"));
            typeof(TreatmentViewerController)
                .GetField("_toothMaterial", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(controller, material);

            _viewerGO.SetActive(true);
            return controller;
        }

        [UnityTest]
        public IEnumerator SelectingATooth_PinsStageTimeToFinalStage()
        {
            var controller = BuildViewer();
            yield return null;

            controller.Select(new ToothId(1, 1));

            Assert.AreEqual(controller.StageCount - 1, controller.StageTime, 0.001f,
                "Selecting a tooth should pin the view to the final stage.");
        }

        [UnityTest]
        public IEnumerator DeselectingRestoresAutoPlay_StageTimeCanLeaveFinalStage()
        {
            var controller = BuildViewer();
            yield return null;

            controller.Select(new ToothId(1, 1));
            controller.Select(null);

            for (int i = 0; i < 5; i++) yield return null;

            Assert.Less(controller.StageTime, controller.StageCount - 1,
                "Deselecting should resume auto-play and let the stage time advance.");
        }

        [UnityTest]
        public IEnumerator RevisingSelectedTooth_ChangesItsVisiblePoseImmediately()
        {
            var controller = BuildViewer();
            var target = new ToothId(1, 1);
            yield return null;

            controller.Select(target);
            controller.TryGetViewTransform(target, out var toothT);
            var before = toothT.rotation;

            controller.ReviseSelected(Vector3.zero, Quaternion.AngleAxis(30f, Vector3.up));
            yield return null;

            var after = toothT.rotation;
            Assert.Greater(Quaternion.Angle(before, after), 1f,
                "A rotation revision should immediately change the selected tooth's visible pose.");
        }
    }
}

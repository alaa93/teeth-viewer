using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace AlaAbuhadbeh.TeethViewer.Editor
{
    public static class TeethViewerSceneBuilder
    {
        private const string ShaderPath = "TeethViewer/Tooth";
        private const string MaterialFolder = "Assets/TeethViewer";
        private const string MaterialPath = MaterialFolder + "/ToothMaterial.mat";

        [MenuItem("TeethViewer/Build Demo Scene")]
        public static void BuildDemoScene()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            var cameraGO = new GameObject("Main Camera", typeof(Camera), typeof(OrbitCamera));
            cameraGO.tag = "MainCamera";
            cameraGO.transform.position = new Vector3(0f, 1.2f, -6f);

            var lightGO = new GameObject("Directional Light", typeof(Light));
            var light = lightGO.GetComponent<Light>();
            light.type = LightType.Directional;
            light.transform.rotation = Quaternion.Euler(45f, -30f, 0f);
            light.intensity = 1.1f;

            var viewerGO = new GameObject("Treatment Viewer", typeof(TreatmentViewerController), typeof(ToothSelector));
            var controller = viewerGO.GetComponent<TreatmentViewerController>();
            var so = new SerializedObject(controller);
            so.FindProperty("_toothMaterial").objectReferenceValue = GetOrCreateToothMaterial();
            so.ApplyModifiedPropertiesWithoutUndo();

            new GameObject("Controls Overlay", typeof(ControlsOverlay));

            EditorSceneManager.SaveScene(scene, "Assets/TeethViewer/TeethViewerDemo.unity");
            Debug.Log("TeethViewer demo scene built. Press Play to see staged tooth movement; " +
                      "left-click selects a tooth, right-drag rotates it about its own axis, " +
                      "middle-drag orbits the camera.");
        }

        private static Material GetOrCreateToothMaterial()
        {
            if (!AssetDatabase.IsValidFolder(MaterialFolder))
                AssetDatabase.CreateFolder("Assets", "TeethViewer");

            var existing = AssetDatabase.LoadAssetAtPath<Material>(MaterialPath);
            if (existing != null)
                return existing;

            var shader = Shader.Find(ShaderPath);
            if (shader == null)
            {
                Debug.LogError($"Could not find shader '{ShaderPath}'. Has the package compiled?");
                return null;
            }

            var material = new Material(shader) { name = "ToothMaterial" };
            AssetDatabase.CreateAsset(material, MaterialPath);
            AssetDatabase.SaveAssets();
            return material;
        }
    }
}

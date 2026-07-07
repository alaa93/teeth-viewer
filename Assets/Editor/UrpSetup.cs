using UnityEditor;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace AlaAbuhadbeh.TeethViewer.DemoProject.Editor
{
    public static class UrpSetup
    {
        private const string RendererPath = "Assets/TeethViewer/DemoRendererData.asset";
        private const string PipelineAssetPath = "Assets/TeethViewer/DemoUrpAsset.asset";

        public static void EnsureActive()
        {
            if (!AssetDatabase.IsValidFolder("Assets/TeethViewer"))
                AssetDatabase.CreateFolder("Assets", "TeethViewer");

            var rendererData = AssetDatabase.LoadAssetAtPath<UniversalRendererData>(RendererPath);
            if (rendererData == null)
            {
                rendererData = ScriptableObject.CreateInstance<UniversalRendererData>();
                AssetDatabase.CreateAsset(rendererData, RendererPath);
            }

            var pipelineAsset = AssetDatabase.LoadAssetAtPath<UniversalRenderPipelineAsset>(PipelineAssetPath);
            if (pipelineAsset == null)
            {
                pipelineAsset = UniversalRenderPipelineAsset.Create(rendererData);
                AssetDatabase.CreateAsset(pipelineAsset, PipelineAssetPath);
            }

            GraphicsSettings.defaultRenderPipeline = pipelineAsset;
            QualitySettings.renderPipeline = pipelineAsset;

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"URP is now the active render pipeline: {AssetDatabase.GetAssetPath(pipelineAsset)}");
        }
    }
}

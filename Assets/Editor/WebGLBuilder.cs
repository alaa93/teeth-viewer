using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace AlaAbuhadbeh.TeethViewer.DemoProject.Editor
{
    public static class WebGLBuilder
    {
        public static void Build()
        {
            PlayerSettings.WebGL.decompressionFallback = true;

            var options = new BuildPlayerOptions
            {
                scenes = new[] { "Assets/TeethViewer/TeethViewerDemo.unity" },
                locationPathName = "Builds/WebGL",
                target = BuildTarget.WebGL,
                options = BuildOptions.None
            };

            BuildReport report = BuildPipeline.BuildPlayer(options);

            if (report.summary.result != BuildResult.Succeeded)
                throw new BuildFailedException($"WebGL build failed: {report.summary.result}, {report.summary.totalErrors} errors");
        }
    }
}

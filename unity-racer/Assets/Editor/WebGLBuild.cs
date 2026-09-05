#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace ProjectStar.Racer.Editor
{
    public static class WebGLBuild
    {
        [MenuItem("ProjectStar/Build WebGL Vertical Slice")]
        public static void Build()
        {
            const string scenePath = "Assets/Scenes/MumbaiVerticalSlice.unity";
            if (!File.Exists(scenePath)) MumbaiVerticalSliceBuilder.Build();

            Directory.CreateDirectory("Build/WebGL");
            PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Brotli;
            PlayerSettings.WebGL.decompressionFallback = true;
            PlayerSettings.runInBackground = true;
            PlayerSettings.defaultScreenWidth = 1170;
            PlayerSettings.defaultScreenHeight = 2532;

            var options = new BuildPlayerOptions
            {
                scenes = new[] { scenePath },
                locationPathName = "Build/WebGL",
                target = BuildTarget.WebGL,
                options = BuildOptions.None
            };

            var report = BuildPipeline.BuildPlayer(options);
            if (report.summary.result != BuildResult.Succeeded)
                throw new System.Exception($"ProjectStar WebGL build failed: {report.summary.result}");

            Debug.Log($"ProjectStar WebGL build complete: {report.summary.totalSize / 1048576f:0.0} MB");
        }
    }
}
#endif

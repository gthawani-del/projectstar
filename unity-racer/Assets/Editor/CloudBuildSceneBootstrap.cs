#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace ProjectStar.Racer.Editor
{
    [InitializeOnLoad]
    public static class CloudBuildSceneBootstrap
    {
        private const string ScenePath = "Assets/Scenes/MumbaiVerticalSlice.unity";

        static CloudBuildSceneBootstrap()
        {
            EnsureBuildScene();
        }

        [InitializeOnLoadMethod]
        private static void EnsureBuildScene()
        {
            var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(ScenePath);
            if (sceneAsset == null)
            {
                Debug.LogError($"ProjectStar Cloud Build: required scene not found at {ScenePath}");
                return;
            }

            var current = EditorBuildSettings.scenes;
            if (current != null && current.Length == 1 && current[0].enabled && current[0].path == ScenePath)
            {
                Debug.Log($"ProjectStar Cloud Build: build scene already configured: {ScenePath}");
                return;
            }

            EditorBuildSettings.scenes = new[]
            {
                new EditorBuildSettingsScene(ScenePath, true)
            };

            AssetDatabase.SaveAssets();
            Debug.Log($"ProjectStar Cloud Build: enforced build scene: {ScenePath}");
        }
    }
}
#endif

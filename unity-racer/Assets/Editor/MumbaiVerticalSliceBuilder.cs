#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

namespace ProjectStar.Racer.Editor
{
    public static class MumbaiVerticalSliceBuilder
    {
        [MenuItem("ProjectStar/Build Mumbai Vertical Slice")]
        public static void Build()
        {
            EnsureFolder("Assets/Scenes");
            EnsureFolder("Assets/Materials");

            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            var root = new GameObject("PROJECTSTAR_MUMBAI_VERTICAL_SLICE");

            var world = new GameObject("World");
            world.transform.SetParent(root.transform);

            var road = GameObject.CreatePrimitive(PrimitiveType.Plane);
            road.name = "ROAD_PLACEHOLDER_REPLACE_WITH_PBR_ASSET";
            road.transform.SetParent(world.transform);
            road.transform.localScale = new Vector3(4.5f, 1f, 25f);
            road.GetComponent<Renderer>().sharedMaterial = CreatePlaceholderRoadMaterial();

            var player = GameObject.CreatePrimitive(PrimitiveType.Cube);
            player.name = "PLAYER_PLACEHOLDER_REPLACE_WITH_PREMIUM_VEHICLE";
            player.transform.SetParent(root.transform);
            player.transform.position = new Vector3(0f, .6f, -5f);
            player.transform.localScale = new Vector3(1.8f, .7f, 4.2f);

            var cameraObject = new GameObject("Main Camera");
            cameraObject.tag = "MainCamera";
            var camera = cameraObject.AddComponent<Camera>();
            cameraObject.transform.position = new Vector3(0f, 3.1f, -10.5f);
            cameraObject.transform.rotation = Quaternion.Euler(11f, 0f, 0f);
            camera.fieldOfView = 67f;
            camera.allowHDR = true;

            var sunObject = new GameObject("Monsoon Key Light");
            var sun = sunObject.AddComponent<Light>();
            sun.type = LightType.Directional;
            sun.intensity = 1.25f;
            sun.shadows = LightShadows.Soft;
            sunObject.transform.rotation = Quaternion.Euler(32f, -28f, 0f);

            RenderSettings.ambientMode = AmbientMode.Trilight;
            RenderSettings.ambientSkyColor = new Color(.18f, .22f, .3f);
            RenderSettings.ambientEquatorColor = new Color(.08f, .1f, .14f);
            RenderSettings.ambientGroundColor = new Color(.025f, .03f, .04f);
            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.ExponentialSquared;
            RenderSettings.fogColor = new Color(.07f, .09f, .12f);
            RenderSettings.fogDensity = .0085f;

            var specObject = new GameObject("GameSpec");
            specObject.transform.SetParent(root.transform);
            specObject.AddComponent<GameSpecLoader>();

            EditorSceneManager.SaveScene(scene, "Assets/Scenes/MumbaiVerticalSlice.unity");
            AssetDatabase.SaveAssets();
            Debug.Log("Mumbai vertical slice scaffold created. Do not visual-review until premium vehicle, road, streetscape and VFX assets are installed.");
        }

        private static Material CreatePlaceholderRoadMaterial()
        {
            var shader = Shader.Find("Universal Render Pipeline/Lit");
            var material = new Material(shader) { name = "M_WetRoad_PLACEHOLDER" };
            material.SetColor("_BaseColor", new Color(.055f, .06f, .065f));
            material.SetFloat("_Smoothness", .82f);
            material.SetFloat("_Metallic", .08f);
            return material;
        }

        private static void EnsureFolder(string fullPath)
        {
            if (AssetDatabase.IsValidFolder(fullPath)) return;
            var split = fullPath.Split('/');
            var parent = split[0];
            for (var i = 1; i < split.Length; i++)
            {
                var next = parent + "/" + split[i];
                if (!AssetDatabase.IsValidFolder(next)) AssetDatabase.CreateFolder(parent, split[i]);
                parent = next;
            }
        }
    }
}
#endif

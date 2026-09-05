using UnityEngine;

namespace ProjectStar.Racer
{
    public class GameSpecLoader : MonoBehaviour
    {
        [SerializeField] private TextAsset gameSpecJson;
        public RacerGameSpec Spec { get; private set; }

        private void Awake()
        {
            if (gameSpecJson == null)
            {
                Debug.LogError("ProjectStar Racer: GameSpec JSON is not assigned.");
                enabled = false;
                return;
            }

            Spec = JsonUtility.FromJson<RacerGameSpec>(gameSpecJson.text);
            Application.targetFrameRate = Application.isMobilePlatform
                ? Mathf.Max(30, Spec.mobileTargetFps)
                : Mathf.Max(30, Spec.desktopTargetFps);

            Debug.Log($"ProjectStar Racer loaded: {Spec.location} / {Spec.track} / {Spec.weather}");
        }
    }
}

using UnityEngine;

namespace ProjectStar.Racer
{
    public class RainStreakSystem : MonoBehaviour
    {
        public Transform target;
        public int mobileCount = 90;
        public int desktopCount = 150;
        public float fallSpeed = 34f;
        public Vector3 area = new Vector3(24f, 16f, 34f);

        Transform[] drops;

        void Start()
        {
            var count = Application.isMobilePlatform ? mobileCount : desktopCount;
            drops = new Transform[count];
            var material = CreateRainMaterial();

            for (var i = 0; i < count; i++)
            {
                var drop = GameObject.CreatePrimitive(PrimitiveType.Cube);
                drop.name = "RainStreak";
                drop.transform.SetParent(transform, false);
                drop.transform.localScale = new Vector3(.018f, .38f, .018f);
                drop.GetComponent<Renderer>().sharedMaterial = material;
                var collider = drop.GetComponent<Collider>();
                if (collider) Destroy(collider);
                drops[i] = drop.transform;
                ResetDrop(drops[i], true);
            }
        }

        void LateUpdate()
        {
            if (!target || drops == null) return;
            transform.position = target.position + new Vector3(0f, 8f, 10f);

            var fall = fallSpeed * Time.deltaTime;
            for (var i = 0; i < drops.Length; i++)
            {
                var d = drops[i];
                d.localPosition += new Vector3(-fall * .08f, -fall, -fall * .18f);
                if (d.localPosition.y < -area.y * .5f) ResetDrop(d, false);
            }
        }

        void ResetDrop(Transform d, bool randomY)
        {
            d.localPosition = new Vector3(
                Random.Range(-area.x * .5f, area.x * .5f),
                randomY ? Random.Range(-area.y * .5f, area.y * .5f) : area.y * .5f,
                Random.Range(-area.z * .5f, area.z * .5f));
        }

        Material CreateRainMaterial()
        {
            var shader = Shader.Find("Universal Render Pipeline/Unlit") ?? Shader.Find("Unlit/Color");
            var mat = new Material(shader) { name = "RainStreakMaterial" };
            var color = new Color(.58f, .72f, .9f, .42f);
            if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", color);
            else if (mat.HasProperty("_Color")) mat.SetColor("_Color", color);
            return mat;
        }
    }
}

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace ProjectStar.Racer
{
    public static class RacerBootstrap
    {
        static Material roadMat, laneMat, bodyMat, glassMat, darkMat, warmMat, cyanMat, greenMat;
        static Transform player;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void Boot()
        {
            if (GameObject.Find("PROJECTSTAR_RUNTIME") != null) return;
            Application.targetFrameRate = Application.isMobilePlatform ? 45 : 60;
            QualitySettings.vSyncCount = 0;
            SetupRender();
            BuildMaterials();
            var root = new GameObject("PROJECTSTAR_RUNTIME").transform;
            BuildWorld(root);
            player = BuildPlayer(root);
            BuildTraffic(root);
            SetupCamera(player);
            BuildRain(player);
            new GameObject("HUD").AddComponent<RacerHUD>().player = player.GetComponent<ArcadeCarController>();
        }

        static Shader Lit() => Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard");
        static Material Mat(string name, Color c, float smooth, float metal = 0f)
        {
            var m = new Material(Lit()) { name = name };
            if (m.HasProperty("_BaseColor")) m.SetColor("_BaseColor", c); else m.color = c;
            if (m.HasProperty("_Smoothness")) m.SetFloat("_Smoothness", smooth);
            if (m.HasProperty("_Metallic")) m.SetFloat("_Metallic", metal);
            return m;
        }

        static void BuildMaterials()
        {
            roadMat = Mat("WetAsphalt", new Color(.022f,.025f,.032f), .93f, .12f);
            laneMat = Mat("Lane", new Color(.72f,.74f,.7f), .48f);
            bodyMat = Mat("PlayerRed", new Color(.45f,.012f,.018f), .92f, .55f);
            glassMat = Mat("Glass", new Color(.025f,.055f,.075f), .98f, .25f);
            darkMat = Mat("Dark", new Color(.018f,.022f,.028f), .45f, .2f);
            warmMat = Mat("WarmLight", new Color(1f,.55f,.16f), .8f);
            cyanMat = Mat("Cyan", new Color(.02f,.45f,.72f), .8f, .1f);
            greenMat = Mat("Palm", new Color(.04f,.19f,.075f), .35f);
        }

        static GameObject Cube(string name, Vector3 pos, Vector3 scale, Material mat, Transform parent=null)
        {
            var o = GameObject.CreatePrimitive(PrimitiveType.Cube); o.name=name; o.transform.position=pos; o.transform.localScale=scale;
            if(parent) o.transform.SetParent(parent); o.GetComponent<Renderer>().sharedMaterial=mat; return o;
        }

        static void SetupRender()
        {
            RenderSettings.fog = true; RenderSettings.fogMode = FogMode.ExponentialSquared; RenderSettings.fogDensity=.0042f;
            RenderSettings.fogColor = new Color(.025f,.035f,.055f);
            RenderSettings.ambientMode = AmbientMode.Trilight;
            RenderSettings.ambientSkyColor = new Color(.09f,.12f,.19f); RenderSettings.ambientEquatorColor=new Color(.045f,.055f,.08f); RenderSettings.ambientGroundColor=new Color(.012f,.015f,.02f);
            var sun = GameObject.Find("Monsoon Key Light")?.GetComponent<Light>();
            if (sun) { sun.intensity=.75f; sun.color=new Color(.72f,.8f,1f); sun.shadows=LightShadows.Soft; }
            var go = new GameObject("PostFX"); var volume=go.AddComponent<Volume>(); volume.isGlobal=true; volume.priority=10;
            var p=ScriptableObject.CreateInstance<VolumeProfile>(); volume.profile=p;
            var bloom=p.Add<Bloom>(); bloom.active=true; bloom.intensity.Override(.45f); bloom.threshold.Override(1.15f); bloom.scatter.Override(.55f);
            var color=p.Add<ColorAdjustments>(); color.active=true; color.postExposure.Override(-.15f); color.contrast.Override(16f); color.saturation.Override(-5f);
            var vignette=p.Add<Vignette>(); vignette.active=true; vignette.intensity.Override(.18f); vignette.smoothness.Override(.6f);
        }

        static void BuildWorld(Transform root)
        {
            var world=new GameObject("MumbaiMarineDrive").transform; world.SetParent(root);
            Cube("Road",new Vector3(0,-.15f,220),new Vector3(18,.25f,480),roadMat,world);
            Cube("SeaWall",new Vector3(-20,1.2f,220),new Vector3(1.4f,2.7f,480),darkMat,world);
            Cube("Promenade",new Vector3(-17,.05f,220),new Vector3(4,.22f,480),laneMat,world);
            for(int z=-10;z<460;z+=18){
                Cube("Lane",new Vector3(-5.8f,.01f,z),new Vector3(.12f,.02f,7),laneMat,world);
                Cube("Lane",new Vector3(0,.01f,z),new Vector3(.12f,.02f,7),laneMat,world);
                Cube("Lane",new Vector3(5.8f,.01f,z),new Vector3(.12f,.02f,7),laneMat,world);
            }
            for(int z=0;z<450;z+=22){ BuildLamp(new Vector3(-14.7f,0,z),world); BuildLamp(new Vector3(14.7f,0,z),world); }
            for(int z=12;z<455;z+=24){ BuildBuilding(new Vector3(28,0,z),z,world); if(z%48==12) BuildPalm(new Vector3(-15.8f,0,z+7),world); }
            for(int z=65;z<430;z+=95) BuildBillboard(new Vector3(12.8f,4.4f,z),world,z%190==65?cyanMat:warmMat);
            BuildGateway(world); BuildSeaLights(world);
        }

        static void BuildBuilding(Vector3 basePos,int seed,Transform parent)
        {
            float h=Random.Range(12f,32f), w=Random.Range(12f,20f), d=Random.Range(10f,18f);
            var b=Cube("ArtDecoFacade",basePos+new Vector3(0,h*.5f,0),new Vector3(w,h,d),Mat("Facade",new Color(.055f+.02f*(seed%3),.065f,.075f),.32f),parent);
            for(float y=3;y<h-1;y+=3.2f) for(float x=-w*.35f;x<w*.35f;x+=3.2f)
                Cube("Window",b.transform.position+new Vector3(-w*.501f+x,y-h*.5f,0),new Vector3(.05f,1.25f,1.05f),seed%3==0?warmMat:glassMat,b.transform);
            Cube("Canopy",basePos+new Vector3(-w*.55f,2.3f,0),new Vector3(2, .25f, d*.72f),darkMat,parent);
        }

        static void BuildLamp(Vector3 p,Transform parent)
        {
            Cube("LampPole",p+new Vector3(0,3.2f,0),new Vector3(.14f,6.4f,.14f),darkMat,parent);
            var bulb=Cube("LampGlow",p+new Vector3(0,6.25f,0),new Vector3(.45f,.24f,.45f),warmMat,parent);
            var l=bulb.AddComponent<Light>(); l.type=LightType.Point; l.range=10; l.intensity=2.2f; l.color=new Color(1f,.62f,.28f); l.shadows=LightShadows.None;
        }

        static void BuildPalm(Vector3 p,Transform parent)
        {
            Cube("Palm",p+new Vector3(0,2.8f,0),new Vector3(.35f,5.6f,.35f),darkMat,parent);
            for(int i=0;i<6;i++){var leaf=Cube("Leaf",p+new Vector3(0,5.7f,0),new Vector3(.18f,.12f,3.5f),greenMat,parent); leaf.transform.rotation=Quaternion.Euler(-12,i*60,0);}
        }

        static void BuildBillboard(Vector3 p,Transform parent,Material mat)
        {
            Cube("BillboardPole",p-new Vector3(0,2,0),new Vector3(.18f,4,.18f),darkMat,parent);
            Cube("PROJECTSTAR Billboard",p,new Vector3(.35f,4.2f,7.4f),mat,parent);
        }

        static void BuildGateway(Transform parent)
        {
            Cube("StartLeft",new Vector3(-12,3,26),new Vector3(.8f,6,.8f),darkMat,parent);
            Cube("StartRight",new Vector3(12,3,26),new Vector3(.8f,6,.8f),darkMat,parent);
            Cube("PROJECTSTAR MUMBAI",new Vector3(0,6,26),new Vector3(24,.8f,.8f),cyanMat,parent);
        }

        static void BuildSeaLights(Transform parent)
        {
            for(int z=15;z<450;z+=28) Cube("SeaReflection",new Vector3(-31,.02f,z),new Vector3(.12f,.02f,8),z%56==15?warmMat:cyanMat,parent);
        }

        static Transform BuildPlayer(Transform root)
        {
            var car=new GameObject("PLAYER_SUPERCAR"); car.transform.SetParent(root); car.transform.position=new Vector3(0,.65f,5);
            var rb=car.AddComponent<Rigidbody>(); rb.mass=1220; rb.linearDamping=.12f; rb.angularDamping=2.6f; rb.interpolation=RigidbodyInterpolation.Interpolate; rb.centerOfMass=new Vector3(0,-.35f,.15f);
            var box=car.AddComponent<BoxCollider>(); box.size=new Vector3(1.85f,.75f,4.2f); box.center=new Vector3(0,.2f,0);
            Cube("Body",car.transform.position,new Vector3(1.9f,.55f,4.25f),bodyMat,car.transform).transform.localPosition=Vector3.zero;
            var hood=Cube("Hood",Vector3.zero,new Vector3(1.75f,.25f,1.6f),bodyMat,car.transform); hood.transform.localPosition=new Vector3(0,.25f,1.05f);
            var cabin=Cube("Cabin",Vector3.zero,new Vector3(1.55f,.62f,1.65f),glassMat,car.transform); cabin.transform.localPosition=new Vector3(0,.62f,-.35f);
            var wing=Cube("RearWing",Vector3.zero,new Vector3(1.85f,.09f,.35f),darkMat,car.transform); wing.transform.localPosition=new Vector3(0,.55f,-1.85f);
            for(int sx=-1;sx<=1;sx+=2) for(int sz=-1;sz<=1;sz+=2){ var w=GameObject.CreatePrimitive(PrimitiveType.Cylinder); w.name="Wheel"; w.transform.SetParent(car.transform); w.transform.localScale=new Vector3(.42f,.18f,.42f); w.transform.localRotation=Quaternion.Euler(0,0,90); w.transform.localPosition=new Vector3(sx*.92f,-.08f,sz*1.35f); w.GetComponent<Renderer>().sharedMaterial=darkMat; }
            var controller=car.AddComponent<ArcadeCarController>(); controller.maxSpeed=46f; controller.acceleration=20f; controller.steerPower=70f;
            return car.transform;
        }

        static void BuildTraffic(Transform root)
        {
            for(int i=0;i<10;i++){
                float lane=(i%3-1)*5.6f; float z=55+i*34;
                var c=new GameObject("TrafficCar"); c.transform.SetParent(root); c.transform.position=new Vector3(lane,.55f,z);
                Cube("TrafficBody",Vector3.zero,new Vector3(1.75f,.75f,3.8f),i%3==0?warmMat:(i%3==1?darkMat:cyanMat),c.transform).transform.localPosition=Vector3.zero;
                var tc=c.AddComponent<TrafficCar>(); tc.speed=10f+(i%4)*2.3f; tc.laneX=lane;
            }
        }

        static void SetupCamera(Transform target)
        {
            var cam=Camera.main;
            if(!cam)
            {
                var c=new GameObject("Main Camera");
                c.tag="MainCamera";
                cam=c.AddComponent<Camera>();
            }
            cam.fieldOfView=64; cam.allowHDR=true; cam.nearClipPlane=.15f; cam.farClipPlane=650;
            var follow=cam.gameObject.GetComponent<ChaseCamera>() ?? cam.gameObject.AddComponent<ChaseCamera>(); follow.target=target;
        }

        static void BuildRain(Transform target)
        {
            var go=new GameObject("MonsoonRain");
            var rain=go.AddComponent<RainStreakSystem>();
            rain.target=target;
        }
    }
}

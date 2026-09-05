using UnityEngine;

namespace ProjectStar.Racer
{
    [RequireComponent(typeof(Rigidbody))]
    public class ArcadeCarController : MonoBehaviour
    {
        public float maxSpeed=46f, acceleration=20f, brakePower=28f, steerPower=70f;
        public float SpeedKph { get; private set; }
        Rigidbody rb;
        float throttle, steer;

        void Awake(){ rb=GetComponent<Rigidbody>(); }

        void Update()
        {
            float kbSteer=Input.GetAxisRaw("Horizontal");
            float kbThrottle=Input.GetAxisRaw("Vertical");
            float touchSteer=0, touchThrottle=0;
            for(int i=0;i<Input.touchCount;i++){
                var t=Input.GetTouch(i); if(t.phase==TouchPhase.Ended||t.phase==TouchPhase.Canceled) continue;
                if(t.position.y < Screen.height*.68f){
                    if(t.position.x < Screen.width*.33f) touchSteer=-1;
                    else if(t.position.x < Screen.width*.66f) touchSteer=1;
                    else touchThrottle=1;
                }
            }
            steer=Mathf.Clamp(Mathf.Abs(touchSteer)>0?touchSteer:kbSteer,-1,1);
            throttle=Mathf.Clamp(Mathf.Abs(touchThrottle)>0?touchThrottle:kbThrottle,-1,1);
            if(Application.isMobilePlatform && Input.touchCount==0) throttle=.72f;
            SpeedKph=rb.linearVelocity.magnitude*3.6f;
        }

        void FixedUpdate()
        {
            Vector3 local=transform.InverseTransformDirection(rb.linearVelocity);
            float forward=local.z;
            if(throttle>0 && forward < maxSpeed) rb.AddForce(transform.forward*acceleration*rb.mass,ForceMode.Force);
            if(throttle<0) rb.AddForce(-transform.forward*brakePower*rb.mass,ForceMode.Force);
            float steerScale=Mathf.Lerp(.48f,1f,Mathf.Clamp01(Mathf.Abs(forward)/12f));
            transform.Rotate(0,steer*steerPower*steerScale*Time.fixedDeltaTime,0,Space.World);
            local.x*=.82f; local.z=Mathf.Clamp(local.z,-8,maxSpeed); rb.linearVelocity=transform.TransformDirection(local);
            rb.AddForce(Vector3.down*rb.mass*4.5f,ForceMode.Force);
            if(transform.position.y<-2 || Mathf.Abs(transform.position.x)>42) ResetCar();
        }

        public void ResetCar(){ transform.position=new Vector3(0,.8f,Mathf.Max(5,transform.position.z-5)); transform.rotation=Quaternion.identity; rb.linearVelocity=Vector3.zero; rb.angularVelocity=Vector3.zero; }
    }

    public class ChaseCamera : MonoBehaviour
    {
        public Transform target;
        Vector3 velocity;
        void LateUpdate(){
            if(!target)return;
            float speed=target.GetComponent<Rigidbody>()?.linearVelocity.magnitude ?? 0;
            Vector3 desired=target.position-target.forward*(7.2f+speed*.035f)+Vector3.up*(3.15f+speed*.012f);
            transform.position=Vector3.SmoothDamp(transform.position,desired,ref velocity,.12f);
            Vector3 look=target.position+target.forward*(5f+speed*.05f)+Vector3.up*.75f;
            transform.rotation=Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(look-transform.position),1-Mathf.Exp(-9f*Time.deltaTime));
            var cam=GetComponent<Camera>(); if(cam) cam.fieldOfView=Mathf.Lerp(cam.fieldOfView,64f+Mathf.Clamp(speed*.28f,0,12),1-Mathf.Exp(-4f*Time.deltaTime));
        }
    }

    public class TrafficCar : MonoBehaviour
    {
        public float speed=12f,laneX;
        void Update(){ transform.position+=Vector3.forward*speed*Time.deltaTime; var p=transform.position; p.x=Mathf.Lerp(p.x,laneX,Time.deltaTime*2); transform.position=p; if(transform.position.z>500) transform.position=new Vector3(laneX,.55f,-10); }
    }

    public class RainFollow : MonoBehaviour
    {
        public Transform target;
        void LateUpdate(){ if(target) transform.position=target.position+new Vector3(0,16,10); }
    }
}

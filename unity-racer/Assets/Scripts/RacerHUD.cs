using UnityEngine;

namespace ProjectStar.Racer
{
    public class RacerHUD : MonoBehaviour
    {
        public ArcadeCarController player;
        GUIStyle title,big,small,button;
        Texture2D panel;
        void Start(){
            panel=new Texture2D(1,1); panel.SetPixel(0,0,new Color(0,0,0,.42f)); panel.Apply();
            title=Style(18,FontStyle.Bold); big=Style(34,FontStyle.Bold); small=Style(13,FontStyle.Normal); button=Style(26,FontStyle.Bold); button.alignment=TextAnchor.MiddleCenter;
        }
        GUIStyle Style(int size,FontStyle weight){ var s=new GUIStyle(GUI.skin.label); s.fontSize=size; s.fontStyle=weight; s.normal.textColor=Color.white; return s; }
        void OnGUI(){
            float scale=Mathf.Clamp(Screen.width/430f,.8f,1.8f); GUI.matrix=Matrix4x4.TRS(Vector3.zero,Quaternion.identity,new Vector3(scale,scale,1));
            float w=Screen.width/scale,h=Screen.height/scale;
            GUI.DrawTexture(new Rect(14,14,184,76),panel); GUI.Label(new Rect(26,22,170,24),"PROJECTSTAR / MUMBAI",title);
            GUI.Label(new Rect(26,48,110,42),player?Mathf.RoundToInt(player.SpeedKph).ToString():"0",big); GUI.Label(new Rect(98,62,55,20),"KM/H",small);
            GUI.Label(new Rect(w-105,22,90,22),"MONSOON",title); GUI.Label(new Rect(w-105,48,90,20),"MARINE DRIVE",small);
            if(Application.isMobilePlatform){
                GUI.DrawTexture(new Rect(18,h-108,78,78),panel); GUI.DrawTexture(new Rect(108,h-108,78,78),panel); GUI.DrawTexture(new Rect(w-96,h-108,78,78),panel);
                GUI.Label(new Rect(18,h-108,78,78),"‹",button); GUI.Label(new Rect(108,h-108,78,78),"›",button); GUI.Label(new Rect(w-96,h-108,78,78),"GO",button);
            } else GUI.Label(new Rect(20,h-38,380,24),"WASD / ARROWS · mobile touch zones enabled",small);
        }
    }
}

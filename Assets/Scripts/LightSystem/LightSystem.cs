using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightSystem : MonoBehaviour
{   
    private static LightSystem instance;
    private List<Light2D> lightList;
    void Awake(){
        if(instance == null){
            instance = new LightSystem();
            instance.lightList = new List<Light2D>();
        }
        else{
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void debug() {
        Debug.Log("ssss");
    }
    public void AddLight(Light2D light) {
            lightList.Add(light);
     }
    public void RemoveLight(Light2D light) { 
            lightList.Remove(light);
    }

    public bool IsIrradiated(Vector2 position) {
        bool isIrradiated = false;
        foreach (Light2D light in lightList) {
           // Debug.Log("active");
            float radius = 1.0f / 4.0f * light.pointLightInnerRadius + light.pointLightOuterRadius * 3.0f / 4.0f;
            Vector2 lightPosition = new Vector2(light.transform.position.x, light.transform.position.y);
            float lightAngleZ = light.transform.rotation.eulerAngles.z;
            //Debug.Log(lightAngleZ);
            Vector2 lightAngleZ2Vector = new Vector2(Mathf.Sin(lightAngleZ * Mathf.Deg2Rad), Mathf.Cos(lightAngleZ * Mathf.Deg2Rad));
            Vector2 light2Position = new Vector2(-(position - lightPosition).x, (position - lightPosition).y);
            
            //Debug.Log(position.x);
            float dotValue = Vector2.Dot(light2Position.normalized,lightAngleZ2Vector.normalized);
            float angle = 1.0f/ 8.0f * light.pointLightInnerAngle + 3.0f / 8.0f * light.pointLightOuterAngle;
            Debug.Log(dotValue);
            Debug.Log(angle);
            Debug.Log(Mathf.Cos(angle * Mathf.Deg2Rad));
            //&&dotValue > Mathf.Cos(angle)
            if (light2Position.magnitude < radius && dotValue > Mathf.Cos(angle * Mathf.Deg2Rad)) {
                isIrradiated = true;
                break;
            }
        }
        return isIrradiated;
    }
    public static LightSystem Instance{
        get{
            return instance;
        }

    }
}

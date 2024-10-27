using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReflectionManager : MonoBehaviour
{
    public GameObject reflectTextPrefab;
    //public static ReflectionManager Instance;
    public  void Reflect(string reflectText) {
        Text text = Instantiate(reflectTextPrefab,new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0),Quaternion.identity).GetComponentInChildren<Text>();
        text.text = reflectText;
    }
   
}

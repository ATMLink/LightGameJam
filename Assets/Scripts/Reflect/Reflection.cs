using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Unity.Mathematics;

public class Reflection : MonoBehaviour
{
    float time;
    // Start is called before the first frame update
    void Start()
    {
        
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, (1 - time)*2 * Time.deltaTime, 0);
        time = time + Time.deltaTime;
        if (time > 1) { Destroy(gameObject); }
    }
}

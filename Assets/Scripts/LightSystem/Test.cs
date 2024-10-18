using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Test : MonoBehaviour
{
    public Light2D lightdd;
    // Start is called before the first frame update
    void Start()
    {
        LightSystem.Instance.AddLight(lightdd);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(LightSystem.Instance.IsIrradiated(new Vector2(transform.position.x, transform.position.y)));
        //LightSystem.Instance.debug();
    }
}

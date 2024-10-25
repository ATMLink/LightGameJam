using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Wall : Tower
{
    public bool canLightThrough;
    public ShadowCaster2D shadowCaster;
    
    void Start()
    {
        shadowCaster = GetComponent<ShadowCaster2D>();
        if (!canLightThrough)shadowCaster.enabled = true;
    }
}

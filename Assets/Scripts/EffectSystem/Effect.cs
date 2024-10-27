using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    [SerializeField]
    private string effectName;
    [SerializeField]
    private ParticleSystem particle;
    [SerializeField]
    private float duration = 5f;


    protected void OnEnable()
    {
        StartCoroutine(Show());
    }

    protected virtual IEnumerator Show()
    {
        float time = duration;
        while (true)
        {
            time -= Time.deltaTime;
            if (time < 0) break;
            yield return null;
        }
        EffectPool.instance.ReturnObjToPool(this, effectName);
    }



    protected void OnDisable()
    {
        
    }

}

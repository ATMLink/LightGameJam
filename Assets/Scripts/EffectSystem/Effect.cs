using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Effect : MonoBehaviour
{
    [SerializeField]
    protected string effectName;
    [SerializeField]
    protected ParticleSystem particle;
    [SerializeField]
    private float duration = 5f;

    protected Coroutine coroutine;


    protected float skillSaving = 0;

    protected virtual void OnEnable()
    {
        coroutine = StartCoroutine(Show());
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
        if (coroutine != null) {StopCoroutine(coroutine); }
        EffectPool.instance.ReturnObjToPool(this, effectName);
    }


    public virtual void StartEffect(float skill)
    {

    }

    public virtual void EndEffect()
    {

    }

    protected void OnDisable()
    {
        
    }

}

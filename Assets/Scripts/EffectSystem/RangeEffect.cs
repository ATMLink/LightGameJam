using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class RangeEffect : Effect
{

    protected override void OnEnable()
    {

    }

    protected override IEnumerator Show()
    {
        ShapeModule shape = particle.shape;
        var emission = particle.emission;
        emission.rateOverTime = skillSaving * 2 * MathF.PI * 20;
        while (true)
        {
            shape.radius = skillSaving;
            yield return null;
        }
    }


    public override void StartEffect(float skill)
    {
        skillSaving = skill;
        coroutine = StartCoroutine(Show());
    }

    public override void EndEffect()
    {
        if (coroutine != null) { StopCoroutine(coroutine); }
        var emission = particle.emission;
        emission.rateOverTime = 0;
        particle.Clear();
        EffectPool.instance.ReturnObjToPool(this, effectName);
    }


}

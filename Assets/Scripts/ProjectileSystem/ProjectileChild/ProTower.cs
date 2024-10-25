using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ProTower : Projectile
{

    protected override void HitTarget(float damage)
    {
        if(target.gameObject.activeInHierarchy)target.GetComponent<Enemy>().OnHit(damage);
    }

}

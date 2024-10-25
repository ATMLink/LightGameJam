using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProEnemy : Projectile
{
    protected override void HitTarget(float damage)
    {
        if (target.gameObject.activeInHierarchy) target.GetComponent<Tower>().OnHit((int)damage);
    }
}

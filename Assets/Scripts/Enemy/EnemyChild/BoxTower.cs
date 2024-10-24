using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTower : Tower
{
    public override void OnHit(int damage)
    {
        gameObject.transform.parent.GetComponent<Enemy>().OnHit(damage);
    }
}

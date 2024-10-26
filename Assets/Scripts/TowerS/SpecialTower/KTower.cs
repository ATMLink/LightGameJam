using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KTower : TowerProjectile
{

    public override void OnLaserHit(Laser laser)
    {
        if (receivedLasers != null)
        {
            receivedLasers.Add(laser);
        }
        if (!canAttack)
        {
            float inten = -400;
            foreach (var lasr in receivedLasers)
            {
                inten += lasr.intensity;
            }
            if (inten > 30f) canAttack = true;
        }
    }

}

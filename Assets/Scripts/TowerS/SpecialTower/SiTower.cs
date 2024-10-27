using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiTower : TowerProjectile
{

    private float power = 0;

    public override void Attack()
    {
        DamageTest();
        if (!canAttack)
        {
            return;
        }
        if (sight1.EnemyInSight.Count > 0 && attackTimer >= attackCooldown)
        {
            // 攻击最近的敌人
            Enemy target = FindClosestEnemy();
            if (target.gameObject.activeInHierarchy)
            {
                // 对敌人造成伤害
                Projectile pro = ProjectilePool.instance.GetObjFromPool(projectileName);
                pro.transform.position = transform.position;
                pro.Launch(target.transform, damage + power * 5);
                attackTimer = 0f; // 重置攻击计时器
            }
        }
        else
        {
            attackTimer += Time.deltaTime; // 增加计时器
        }
    }
    protected override void DamageTest()
    {
        
    }


    public override void OnLaserHit(Laser laser)
    {
        if (receivedLasers != null)
        {
            receivedLasers.Add(laser);
        }
        if (!canAttack)
        {
            float inten = 0;
            foreach (var lasr in receivedLasers)
            {
                inten += lasr.intensity;
            }

            Debug.Log(inten);
            if (inten >= 20f)
            {
                canAttack = true;
                power = inten / 20;
                if (power > 3) power = 3;
            }
            else
            {
                canAttack = false;
                power = 0;
            }
        }
    }

    public override void OnLaserOut(Laser laser)
    {
        if (receivedLasers.Contains(laser))
        {
            receivedLasers.Remove(laser);
        }
        if (canAttack)
        {
            float inten = 0;
            foreach (var lasr in receivedLasers)
            {
                inten += lasr.intensity;
            }
            if (inten < 20f) canAttack = false;
        }
    }
}

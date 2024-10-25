using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class CuTower : Tower
{
    [SerializeField]
    private ParticleSystem particle;

    protected bool canAttack = false;

    public override void Attack()
    {
        DamageTest();
        if (attackTimer < attackCooldown)attackTimer += Time.deltaTime; // 增加计时器
        if (sight1.EnemyInSight.Count > 0)
        {
            var emission = particle.emission;
            emission.rateOverTime = 200;
            if (attackTimer >= attackCooldown)
            {
                // 攻击所有敌人
                foreach(var enemy in sight1.EnemyInSight)
                if (enemy != null)
                {
                    enemy.OnHit(damage); // 对敌人造成伤害
                    attackTimer = 0f; // 重置攻击计时器
                }
            }
        }
        else
        {
            var emission = particle.emission;
            emission.rateOverTime = 0;
        }
    }

    protected virtual void DamageTest()
    {
        if (!canAttack) return;
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
            if (inten > 30f) canAttack = true;
        }
    }

    public override void OnLaserOut(Laser laser)
    {
        if (receivedLasers.Contains(laser))
        {
            laser.UpdateState();
            receivedLasers.Remove(laser);
        }
    }



    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Laser"))
        {
            Laser laser = collision.GetComponent<Laser>();
            if (laser != null)
            {
                OnLaserHit(laser);
            }
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Laser"))
        {
            Laser laser = collision.GetComponent<Laser>();
            if (laser != null)
            {
                OnLaserOut(laser);
            }
        }
    }
}

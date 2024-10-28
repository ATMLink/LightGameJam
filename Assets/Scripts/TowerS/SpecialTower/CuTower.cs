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

    public override void Initialize()
    {
        base.Initialize();
        var emission = particle.emission;
        emission.rateOverTime = 0;
    }

    public override void Attack()
    {
        DamageTest();
        if (!canAttack) return;
        if (attackTimer < attackCooldown)attackTimer += Time.deltaTime; // 增加计时器
        if (sight1.EnemyInSight.Count > 0)
        {
            var emission = particle.emission;
            emission.rateOverTime = 200;
            if (attackTimer >= attackCooldown)
            {
                // 攻击所有敌人
                for(int i = 0; i < sight1.EnemyInSight.Count; i++)
                {
                    if (sight1.EnemyInSight[i] != null)
                    {
                        sight1.EnemyInSight[i].OnHit(damage); // 对敌人造成伤害
                        attackTimer = 0f; // 重置攻击计时器
                    }
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
            if (inten >= 30f) canAttack = true;
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
            if (inten < 30f) canAttack = false;
        }
    }
}

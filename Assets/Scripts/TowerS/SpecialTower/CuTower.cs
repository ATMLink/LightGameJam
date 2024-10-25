using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class CuTower : Tower
{
    [SerializeField]
    private ParticleSystem particle;


    public override void Attack()
    {
        if(attackTimer < attackCooldown)attackTimer += Time.deltaTime; // 增加计时器
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



}

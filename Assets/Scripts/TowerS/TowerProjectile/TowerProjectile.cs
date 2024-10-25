using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;  

public class TowerProjectile : Tower
{
    [SerializeField]
    private string projectileName;

    public override void Attack()
    {
        if (sight1.EnemyInSight.Count > 0 && attackTimer >= attackCooldown)
        {
            // 攻击最近的敌人
            Enemy target = FindClosestEnemy();
            if (target != null)
            {
                // 对敌人造成伤害
                Projectile pro = ProjectilePool.instance.GetObjFromPool(projectileName);
                pro.transform.position = transform.position;
                pro.Launch(target.transform, damage);
                attackTimer = 0f; // 重置攻击计时器
            }
        }
        else
        {
            attackTimer += Time.deltaTime; // 增加计时器
        }
    }
}

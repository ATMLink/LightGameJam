using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KTower : TowerProjectile
{



    public override void Attack()
    {
        DamageTest();
        if (!canAttack)
        {
            return;
        }

        switch (state)
        {
            case TowerState.idle:

                attackTimer = attackCooldown;

                Turn(Or);
                if (sight1.EnemyInSight.Count > 0)
                {
                    target = FindClosestEnemy();
                    if (target != null && target.gameObject.activeInHierarchy) state = TowerState.attack;
                }

                break;
            case TowerState.attack:

                if (attackTimer >= attackCooldown)
                {

                }
                else
                {
                    attackTimer += Time.deltaTime; // 增加计时器
                }

                if (sight1.EnemyInSight.Count > 0)
                {
                    // 攻击最近的敌人
                    target = FindClosestEnemy();
                }
                if (target != null && target.gameObject.activeInHierarchy)
                {
                    // 计算目标与当前物体之间的方向
                    Vector3 direction = (target.transform.position - transform.position).normalized;

                    if (direction != Vector3.zero) // 确保方向不为零
                    {
                        // 计算目标旋转
                        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);

                        // 平滑旋转到目标旋转
                        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 5 * Time.deltaTime);
                    }
                    // 获取物体的朝向
                    Vector3 forwardDirection = new Vector3(0, 0, transform.position.z);

                    // 计算目标物体与自身的方向向量
                    Vector3 directionToTarget = target.transform.position - transform.position;
                    float angle = Vector3.Angle(forwardDirection, directionToTarget);

                    if (angle < 5 && attackTimer >= attackCooldown)
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
                    state = TowerState.idle;
                    Or = transform.eulerAngles.z;
                }
                break;
        }

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

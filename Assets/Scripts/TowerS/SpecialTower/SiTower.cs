using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SiTower : TowerProjectile
{

    private float power = 0;

    private float routerSupply = 0;

    private float routerCount = 0;
    private float RouterCount
    {
        get {return routerCount;}
        set
        {
            routerCount = value;
            if (routerCount > 0)
            {
                canAttack = true;
                routerSupply = 20;
            }
            else
            {
                routerSupply = 0;
                if (!canAttack)
                {
                    float inten = routerSupply;
                    foreach (var lasr in receivedLasers)
                    {
                        inten += lasr.intensity;
                    }
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
        }
    }

    public override void Initialize()
    {
        base.Initialize();
        RouterCount = 0;
    }

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
                        pro.Launch(target.transform, damage + power * 5);
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

    protected override void RouterSupply()
    {
        if (RouterCount != routerTowerList.Count)
        {
            RouterCount = routerTowerList.Count;
        }
    }


    protected override void DamageTest()
    {
        
    }


    public override void RemoveRouterToTower(RouterTower router)
    {
        if (routerTowerList.Contains(router))
        {
            routerTowerList.Remove(router);
            if (routerTowerList.Count == 0)
            {
                routerSupply = 0;
                if (canAttack)
                {
                    float inten = routerSupply;
                    foreach (var lasr in receivedLasers)
                    {
                        inten += lasr.intensity;
                    }
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
            float inten = routerSupply;
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
            float inten = routerSupply;
            foreach (var lasr in receivedLasers)
            {
                inten += lasr.intensity;
            }
            if (inten < 20f) canAttack = false;
        }
    }
}

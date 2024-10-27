using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;  

public class TowerProjectile : Tower
{
    [SerializeField]
    protected string projectileName;

    protected bool canAttack = false;


    public override void UpdateState()
    {
        foreach(var laser in receivedLasers)
        {
            Debug.Log($"收到来自{laser.name}的强度{laser.intensity}");
        }

        Attack();
    }
    public override void Attack()
    {
        DamageTest();
        if (!canAttack) return;
        if (sight1.EnemyInSight.Count > 0 && attackTimer >= attackCooldown)
        {
            // 攻击最近的敌人
            Enemy target = FindClosestEnemy();
            if (target.gameObject.activeInHierarchy)
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

    protected virtual void DamageTest()
    {
        //弃用
    }


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
            if (inten > 20f) canAttack = true;
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


    //protected void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Laser"))
    //    {
    //        Laser laser = collision.gameObject.GetComponent<Laser>();
    //        if (laser != null)
    //        {
    //            OnLaserHit(laser);
    //        }
    //    }
    //}

    //protected void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Laser"))
    //    {
    //        Laser laser = collision.gameObject.GetComponent<Laser>();
    //        if (laser != null)
    //        {
    //            OnLaserOut(laser);
    //        }
    //    }
    //}

    //protected void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Laser"))
    //    {
    //        Laser laser = collision.GetComponent<Laser>();
    //        if (laser != null)
    //        {
    //            OnLaserHit(laser);
    //        }
    //    }
    //}

    //protected void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Laser"))
    //    {
    //        Laser laser = collision.GetComponent<Laser>();
    //        if (laser != null)
    //        {
    //            OnLaserOut(laser);
    //        }
    //    }
    //}

}

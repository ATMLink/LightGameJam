using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;  

public class TowerProjectile : Tower
{
    [SerializeField]
    protected string projectileName;

    protected bool canAttack = false;


    protected Enemy target;


    protected enum TowerState{
        idle,
        attack,
    }

    protected TowerState state = TowerState.idle;

    protected bool inTurn = false;
    protected float stayTime = 0;
    protected Quaternion Tr;
    protected float Or;//原始角度


    public override void Initialize()
    {
        base.Initialize();
        Or = 0;
    }

    public override void UpdateState()
    {
        //foreach(var laser in receivedLasers)
        //{
        //    Debug.Log($"收到来自{laser.name}的强度{laser.intensity}");
        //}

        Attack();
        RouterSupply();
    }
    public override void Attack()
    {
        DamageTest();
        if (!canAttack) return;

        if(attackTimer >= attackCooldown)
        {

        }
        else
        {
            attackTimer += Time.deltaTime; // 增加计时器
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
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 1 * Time.deltaTime);
            }
            // 获取物体的朝向
            Vector3 forwardDirection = new Vector3(0, 0, transform.position.z);

            // 计算目标物体与自身的方向向量
            Vector3 directionToTarget = target.transform.position - transform.position;
            float angle = Vector3.Angle(forwardDirection, directionToTarget);

            if (angle < 30)
            {
                // 对敌人造成伤害
                Projectile pro = ProjectilePool.instance.GetObjFromPool(projectileName);
                pro.transform.position = transform.position;
                pro.Launch(target.transform, damage);
                attackTimer = 0f; // 重置攻击计时器
            }

        }
        if (sight1.EnemyInSight.Count > 0)
        {
            // 攻击最近的敌人
            target = FindClosestEnemy();
        }

    }

    public override void OnRotateEnd()
    {
        base.OnRotateEnd();
        Or = transform.eulerAngles.z;
    }

    protected virtual void DamageTest()
    {
        //弃用
    }



    protected virtual void Turn(float r, float rR = 45, float minSearchingTime = 4, float maxSearchingTime = 25)
    {
        //基于什么角度为中心进行旋转侦察，侦察角度
        if (inTurn == true)
        {
            //允许旋转且未找到时执行
            transform.rotation = Quaternion.Slerp(transform.rotation, Tr, Time.deltaTime * 2);
            if (Quaternion.Angle(Tr, transform.rotation) < 1)
            {
                transform.rotation = Tr;
                //当物体当前角度与目标角度差值小于1度直接让旋转角度精确到我们想要的度数
                inTurn = false;
                stayTime = Random.Range(minSearchingTime * 1f, maxSearchingTime * 1f);
            }

        }
        else
        {
            if (stayTime > 0)
            {
                stayTime -= Time.deltaTime;
            }
            else
            {

                Tr = Quaternion.Euler(0, 0, r + Random.Range(-rR, rR)) * Quaternion.identity;
                //确定下一个旋转位置
                inTurn = true;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Creeper : Enemy
{

    private float boomDamage = 0;
    private float boomCD = 0;

    private EnemySight2 sight2;


    protected override void Start()
    {
        //视野触发器2,用于获取范围内的我方单位
        sight2 = gameObject.transform.GetChild(1).GetComponent<EnemySight2>();

        //需要写在Start前
        base.Start();
    }

    protected override void ExtraEnableSet()
    {
        sight2.enabled = false;
    }

    protected override void ExtraDisableSet()
    {
        base.ExtraDisableSet();
        sight1 = null;
    }


    protected override void CopyData()
    {
        base.CopyData();
        boomDamage = saving.specialAbility[0];
        boomCD = saving.specialAbility[2];
    }



    protected override void SkillPlan()
    {
        if (boomCD > 0)
        {
            boomCD -= Time.deltaTime;
        }
        else
        {
            foreach (var tower in sight2.towerInSight)
            {
                tower.OnHit((int)boomDamage);
            }

            foreach (var enemy in sight2.enemyInSight)
            {
                if(enemy.gameObject.activeInHierarchy == true)
                {
                    enemy.OnHit(boomDamage);
                }
            }
            ReturnToPool();
        }
    }


    protected override void Destroy()
    {
        sight2.enabled = true;
        enemyState = EnemyState.skill;
    }
}

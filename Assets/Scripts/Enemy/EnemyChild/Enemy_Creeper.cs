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
        //��Ұ������2,���ڻ�ȡ��Χ�ڵ��ҷ���λ
        sight2 = gameObject.transform.GetChild(1).GetComponent<EnemySight2>();

        //��Ҫд��Startǰ
        base.Start();
    }

    protected override void ExtraEnableSet()
    {
        base.ExtraEnableSet();
        sight2.gameObject.SetActive(false);
        sight2.towerInSight.Clear();
        sight2.enemyInSight.Clear();
    }

    protected override void ExtraDisableSet()
    {
        base.ExtraDisableSet();
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
        sight2.gameObject.SetActive(true);
        enemyState = EnemyState.skill;
    }
}

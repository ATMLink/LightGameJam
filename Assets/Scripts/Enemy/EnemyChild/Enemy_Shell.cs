using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Shell : Enemy
{

    private float creationHealth = 0;

    protected override void Start()
    {
        base.Start();
        //视野触发器2,用于获取范围内的我方单位
    }

    protected override void CopyData()
    {
        base.CopyData();
        creationHealth = saving.specialAbility[0];
    }



    protected override void SkillPlan()
    {
        //这里调用一下创造中立障碍物的函数
        ReturnToPool();
    }


    protected override void Destroy()
    {
        enemyState = EnemyState.skill;
    }
}
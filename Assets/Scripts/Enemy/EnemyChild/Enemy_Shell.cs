using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Shell : Enemy
{

    private float creationHealth = 0;

    [SerializeField]
    private string creationName;

    protected override void Start()
    {
        base.Start();
        //��Ұ������2,���ڻ�ȡ��Χ�ڵ��ҷ���λ
    }

    protected override void CopyData()
    {
        base.CopyData();
        creationHealth = saving.specialAbility[0];
    }



    protected override void SkillPlan()
    {
        //�������һ�´��������ϰ���ĺ���
        GenerateEnemy(creationName);

        ReturnToPool();
    }

    private void GenerateEnemy(string name)
    {
        GameObject enemy = EnemyPool.instance.GetEnemyFromPool(name);
        enemy.gameObject.transform.position = transform.position;
        enemy.GetComponent<Enemy>().SetMap(targetPoint);
        EnemyEventSystem.instance.EnemyGenerate(enemy.GetComponent<Enemy>());
    }

    protected override void Destroy()
    {
        enemyState = EnemyState.skill;
    }
}
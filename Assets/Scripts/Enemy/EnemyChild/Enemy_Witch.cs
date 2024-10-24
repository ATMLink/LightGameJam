using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Witch : EnemyRemote
{
    [SerializeField]
    private string creationName;

    Vector3 generateOffset = Vector3.zero;
    private float maxOffsetForGeneration = 0.1f;

    private float creationCount = 0;
    private float maxEnemyCreateCD = 0;
    private float maxEnemyDurCD = 0;

    private float currentCreationCount = 0;
    private float currentEnemyCreateCD = 0;
    private float currentEnemyDurCD = 0;

    protected override void CopyData()
    {
        base.CopyData();
        maxAttackCost = saving.specialAbility[0];
        creationCount = saving.specialAbility[1];
        maxEnemyDurCD = saving.specialAbility[2];
        maxEnemyCreateCD = saving.specialAbility[3];
    }

    protected override void GlobleSkill()
    {
        if(enemyState != EnemyState.skill)
        {
            if (currentEnemyCreateCD > 0)
            {
                currentEnemyCreateCD -= Time.deltaTime;
            }
            else
            {
                enemyState = EnemyState.skill;
                currentEnemyCreateCD = maxEnemyCreateCD;
            }
        }

    }


    protected override void AttackPlan()
    {
        base.AttackPlan();
    }

    protected override void SkillPlan()
    {
        if (currentCreationCount > creationCount)
        {
            currentCreationCount = 0;
            enemyState = EnemyState.move;
        }
        else
        {

            if (currentEnemyDurCD > 0)
            {
                currentEnemyDurCD -= Time.deltaTime;
            }
            else
            {
                GenerateOffsetReset();
                GenerateEnemy(creationName);
                currentEnemyDurCD = maxEnemyDurCD;
                currentCreationCount++;
            }
        }
    }

    protected override void StunPlan()
    {
        base.StunPlan();
        currentCreationCount = maxEnemyCreateCD;
        currentEnemyDurCD = maxEnemyDurCD;
    }


    protected override void FixAttack()
    {
        base.FixAttack();
    }


    private void GenerateEnemy(string name)
    {
        GameObject enemy = EnemyPool.instance.GetEnemyFromPool(name);
        enemy.gameObject.transform.position = transform.position + generateOffset;
        enemy.GetComponent<Enemy>().SetMap(targetPoint);
        EnemyEventSystem.instance.EnemyGenerate(enemy.GetComponent<Enemy>());
    }

    private void GenerateOffsetReset()
    {
        float offsetAngle = Random.Range(0, 360f);
        float radians = Mathf.Deg2Rad * offsetAngle;

        generateOffset = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)).normalized;

        float offsetToCenter = EnemyData.GenerateStrategy(Random.Range(0, maxOffsetForGeneration));

        generateOffset *= offsetToCenter;
    }

}

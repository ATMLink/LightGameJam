using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public TowerAttributes attributes;

    private float health;
    private float damage;
    private float attackSpeed;
    private float attackRange;

    public void Initialize()
    {
        health = attributes.health.Value;
        damage = attributes.damage.Value;
        attackSpeed = attributes.attackSpeed.Value;
        attackRange = attributes.attackRange.Value;

        gameObject.SetActive(true);
    }

    // 重置塔的属性，方便对象池回收
    public void ResetAttributes()
    {
        health = 0;
        damage = 0;
        attackSpeed = 0;
        attackRange = 0;
    }

    public void Upgrade()
    {
        if (attributes.nextLevelAttributes != null)
        {
            attributes = attributes.nextLevelAttributes;
            Initialize();
        }
        else
        {
            Debug.Log("已经达到最高等级，无法继续升级。");
        }
    }

    public void DestroyTower()
    {
        gameObject.SetActive(false); // 将塔移回对象池
    }

    public void Attack()
    {
        // attack logic
    }
}

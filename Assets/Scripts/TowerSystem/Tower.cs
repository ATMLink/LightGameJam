using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public TowerAttributes attributes; // 共享的塔属性（ScriptableObject）

    private float health;
    private float damage;
    private float attackSpeed;
    private float attackRange;

    public void Initialize()
    {
        // 初始化塔的属性
        health = attributes.health.Value;
        damage = attributes.damage.Value;
        attackSpeed = attributes.attackSpeed.Value;
        attackRange = attributes.attackRange.Value;
        
        gameObject.SetActive(true);
    }

    // 升级塔的方法
    public void Upgrade()
    {
        if (attributes.nextLevelAttributes != null)
        {
            // 升级到下一级塔的属性
            attributes = attributes.nextLevelAttributes;
            Initialize(); // 使用新的属性重新初始化塔
        }
        else
        {
            Debug.Log("已经达到最高等级，无法继续升级。");
        }
    }

    // 摧毁塔的方法
    public void DestroyTower()
    {
        // 将塔移回对象池
        gameObject.SetActive(false);
    }
}

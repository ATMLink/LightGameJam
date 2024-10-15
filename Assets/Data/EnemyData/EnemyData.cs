using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public struct EnemyInfo
{
    //依次为攻击力，生命值，攻击速度，攻击范围，移速,嘲讽等级
    public float attack;
    public float health;
    public float attackSpeed;//攻击次数每秒
    public float attackRange;
    public float moveSpeed;

    //嘲讽等级越高,越优先被攻击，反之亦然。默认嘲讽等级是100
    public int priority;

    public List<float> specialAbility;

    public EnemyInfo(float attack, float health, float attackSpeed, float attackRange, float moveSpeed, int priority = 100)
    {
        this.attack = attack * EnemyData.globleGenerateOffset;
        this.health = health * EnemyData.globleHealthValue;
        this.attackSpeed = attackSpeed * EnemyData.globleAttackSpeedValue;
        this.attackRange = attackRange * EnemyData.globleAttackRangeValue;
        this.moveSpeed = moveSpeed * EnemyData.globleMoveSpeedValue;
        this.priority = priority;
        specialAbility = null;
    }
    public EnemyInfo(float attack, float health, float attackSpeed, float attackRange, float moveSpeed, List<float> specialAbility)
    {
        this.attack = attack * EnemyData.globleGenerateOffset;
        this.health = health * EnemyData.globleHealthValue;
        this.attackSpeed = attackSpeed * EnemyData.globleAttackSpeedValue;
        this.attackRange = attackRange * EnemyData.globleAttackRangeValue;
        this.moveSpeed = moveSpeed * EnemyData.globleMoveSpeedValue;
        this.priority = 100;
        this.specialAbility = specialAbility;
    }
    public EnemyInfo(float attack, float health, float attackSpeed, float attackRange, float moveSpeed, int priority, List<float> specialAbility)
    {
        this.attack = attack * EnemyData.globleGenerateOffset;
        this.health = health * EnemyData.globleHealthValue;
        this.attackSpeed = attackSpeed * EnemyData.globleAttackSpeedValue;
        this.attackRange = attackRange * EnemyData.globleAttackRangeValue;
        this.moveSpeed = moveSpeed * EnemyData.globleMoveSpeedValue;
        this.priority = priority;
        this.specialAbility = specialAbility;
    }
}


public static class EnemyData
{

    //据说需要一个存储enemyType，但是目前疑似也只有可以阻挡和不可以阻挡激光这两种类型
    //也不需要频繁调整，因此就先搁在这
    public enum EnemyType 
    {
        can,
        cant,
    }
    //路径属性
    //与路径的最大偏移量
    public static float globleGenerateOffset = 4f;

    //全局变量，用于快捷调试所有怪物的属性
    public static float globleAttackValue = 1f;
    public static float globleHealthValue = 1f;
    public static float globleAttackSpeedValue = 1f;
    public static float globleAttackRangeValue = 1f;
    public static float globleMoveSpeedValue = 1f;



    //统一存储enemy信息，每个名称对应顺序怪物
    public static Dictionary<string, EnemyInfo> enemyDic;
    public static List<string> enemyName = new List<string>()
    {
        "enemy_Small",
        "enemy_Medium"
    };

    //以下为各个怪物的具体数值
    //依次为攻击力，生命值，攻击速度，攻击范围，移速

    //enemy_Small
    //static List<float> sp1 = new List<float>(){1f};（例：回血速度，对于任何拥有特殊能力的怪都会将对应属性写在其基础属性上）
    static EnemyInfo enemy_1 = new EnemyInfo(1, 10, 1.5f, 0, 10/*,sp1*/);

    //以上是一个完整的怪物信息

    //enemy_Middle
    static EnemyInfo enemy_2 = new EnemyInfo(2, 20, 1f, 1, 5);

    //static EnemyInfo enemy_Huge = new EnemyInfo(1, 1, 1, 1, 1);

    static EnemyData()
    {
        enemyDic = new Dictionary<string, EnemyInfo>
        {
            {enemyName[0],enemy_1},
            {enemyName[1],enemy_2},
        };
    }

    //感觉直接访问字典也没什么问题
    //public static EnemyInfo GetEnemyInfo(string name)
    //{
    //    if (enemyDic.ContainsKey(name))
    //    {
    //        return enemyDic[name];
    //    }
    //    return new EnemyInfo(-1,-1,-1,-1,-1); //返回 null
    //}


    public static float GenerateStrategy(float x)
    {
        float y = 1 - Mathf.Pow((x / 100), 2);
        y *= globleGenerateOffset;
        return y;
    }

    public static float InfinityStrategy(float level)
    {
        //后续修改为无尽模式的难度曲线
        return level;
    }

}

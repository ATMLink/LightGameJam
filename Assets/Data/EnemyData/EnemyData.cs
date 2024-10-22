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
    public static float globleGenerateOffset = 2f;

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
        "enemy_Medium",
        "enemy_Huge",
        "enemy_Creeper",
        "enemy_Skeleton",
        "enemy_Witch",
        "enemy_Creation",
        "enemy_Guard",
        "enemy_Shell",
    };

    //以下为各个怪物的具体数值
    //依次为攻击力，生命值，攻击速度，攻击范围，移速,嘲讽等级,特殊名单

    //enemy_Small
    //static List<float> sp1 = new List<float>(){1f};（例：回血速度，对于任何拥有特殊能力的怪都会将对应属性写在其基础属性上）
    static EnemyInfo enemy_1 = new EnemyInfo(1, 10, 1.5f, 0, 1/*,sp1*/);

    //以上是一个完整的怪物信息

    //enemy_Middle
    static EnemyInfo enemy_2 = new EnemyInfo(2, 20, 1f, 0, 1);

    //enemy_Huge
    static EnemyInfo enemy_3 = new EnemyInfo(8, 64,0.8f, 0, 1);

    //enemy_Creeper
    static List<float> sp4 = new List<float>() { 25f, 2, 0.5f };//爆炸伤害,爆炸范围,爆炸前摇
    static EnemyInfo enemy_4 = new EnemyInfo(2, 30, 0.7f, 0, 2, sp4);

    //enemy_Skeleton
    //理论上攻击时应该会停下来,但是目前没有攻击间隔和攻击花费时间的区别,因此随便定一个攻击花费时间
    static List<float> sp5 = new List<float>() { 0.2f };//攻击花费时间
    static EnemyInfo enemy_5 = new EnemyInfo(3, 15, 1f, 4, 4, sp5);

    //enemy_Witch
    static List<float> sp6 = new List<float>() { 0.2f, 10, 0.2f, 10f };//攻击花费时间,召唤衍生物数量,召唤多个衍生物间隔,与下一次召唤衍生物间隔
    static EnemyInfo enemy_6 = new EnemyInfo(10, 200, 0.5f, 3.5f, 2, 110, sp6);

    //enemy_Creation
    //Witch衍生物
    static EnemyInfo enemy_7 = new EnemyInfo(1, 5, 1.5f, 0, 1);

    //enemy_Guard
    static EnemyInfo enemy_8 = new EnemyInfo(2, 128, 0.7f, 0, 2, 105);

    //enemy_Shell
    static List<float> sp9 = new List<float>() { 150f};//召唤障碍物血量
    static EnemyInfo enemy_9 = new EnemyInfo(2, 20, 1f, 0, 1, sp9);

    static EnemyData()
    {
        enemyDic = new Dictionary<string, EnemyInfo>
        {
            {enemyName[0],enemy_1},
            {enemyName[1],enemy_2},
            {enemyName[2],enemy_3},
            {enemyName[3],enemy_4},
            {enemyName[4],enemy_5},
            {enemyName[5],enemy_6},
            {enemyName[6],enemy_7},
            {enemyName[7],enemy_8},
            {enemyName[8],enemy_9},
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

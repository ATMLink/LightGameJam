using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public struct EnemyInfo
{
    //����Ϊ������������ֵ�������ٶȣ�������Χ������,����ȼ�
    public float attack;
    public float health;
    public float attackSpeed;//��������ÿ��
    public float attackRange;
    public float moveSpeed;

    //����ȼ�Խ��,Խ���ȱ���������֮��Ȼ��Ĭ�ϳ���ȼ���100
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

    //��˵��Ҫһ���洢enemyType������Ŀǰ����Ҳֻ�п����赲�Ͳ������赲��������������
    //Ҳ����ҪƵ����������˾��ȸ�����
    public enum EnemyType 
    {
        can,
        cant,
    }
    //·������
    //��·�������ƫ����
    public static float globleGenerateOffset = 4f;

    //ȫ�ֱ��������ڿ�ݵ������й��������
    public static float globleAttackValue = 1f;
    public static float globleHealthValue = 1f;
    public static float globleAttackSpeedValue = 1f;
    public static float globleAttackRangeValue = 1f;
    public static float globleMoveSpeedValue = 1f;



    //ͳһ�洢enemy��Ϣ��ÿ�����ƶ�Ӧ˳�����
    public static Dictionary<string, EnemyInfo> enemyDic;
    public static List<string> enemyName = new List<string>()
    {
        "enemy_Small",
        "enemy_Medium"
    };

    //����Ϊ��������ľ�����ֵ
    //����Ϊ������������ֵ�������ٶȣ�������Χ������

    //enemy_Small
    //static List<float> sp1 = new List<float>(){1f};��������Ѫ�ٶȣ������κ�ӵ�����������Ĺֶ��Ὣ��Ӧ����д������������ϣ�
    static EnemyInfo enemy_1 = new EnemyInfo(1, 10, 1.5f, 0, 10/*,sp1*/);

    //������һ�������Ĺ�����Ϣ

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

    //�о�ֱ�ӷ����ֵ�Ҳûʲô����
    //public static EnemyInfo GetEnemyInfo(string name)
    //{
    //    if (enemyDic.ContainsKey(name))
    //    {
    //        return enemyDic[name];
    //    }
    //    return new EnemyInfo(-1,-1,-1,-1,-1); //���� null
    //}


    public static float GenerateStrategy(float x)
    {
        float y = 1 - Mathf.Pow((x / 100), 2);
        y *= globleGenerateOffset;
        return y;
    }

    public static float InfinityStrategy(float level)
    {
        //�����޸�Ϊ�޾�ģʽ���Ѷ�����
        return level;
    }

}

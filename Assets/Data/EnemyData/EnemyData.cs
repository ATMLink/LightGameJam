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
        this.attack = attack * EnemyData.globleAttackValue;
        this.health = health * EnemyData.globleHealthValue;
        this.attackSpeed = attackSpeed * EnemyData.globleAttackSpeedValue;
        this.attackRange = attackRange * EnemyData.globleAttackRangeValue;
        this.moveSpeed = moveSpeed * EnemyData.globleMoveSpeedValue;
        this.priority = priority;
        specialAbility = null;
    }
    public EnemyInfo(float attack, float health, float attackSpeed, float attackRange, float moveSpeed, List<float> specialAbility)
    {
        this.attack = attack * EnemyData.globleAttackValue;
        this.health = health * EnemyData.globleHealthValue;
        this.attackSpeed = attackSpeed * EnemyData.globleAttackSpeedValue;
        this.attackRange = attackRange * EnemyData.globleAttackRangeValue;
        this.moveSpeed = moveSpeed * EnemyData.globleMoveSpeedValue;
        this.priority = 100;
        this.specialAbility = specialAbility;
    }
    public EnemyInfo(float attack, float health, float attackSpeed, float attackRange, float moveSpeed, int priority, List<float> specialAbility)
    {
        this.attack = attack * EnemyData.globleAttackValue;
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

    public static float maxDarkSpeedUp = 8;

    //·������
    //��·�������ƫ����
    public static float globleGenerateOffset = 2f;

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
        "enemy_Medium",
        "enemy_Huge",
        "enemy_Creeper",
        "enemy_Skeleton",
        "enemy_Witch",
        "enemy_Creation",
        "enemy_Guard",
        "enemy_Shell",
        "enemy_Box"
    };

    //����Ϊ��������ľ�����ֵ
    //����Ϊ������������ֵ�������ٶȣ�������Χ������,����ȼ�,��������

    //enemy_Small
    //static List<float> sp1 = new List<float>(){1f};��������Ѫ�ٶȣ������κ�ӵ�����������Ĺֶ��Ὣ��Ӧ����д������������ϣ�
    static EnemyInfo enemy_1 = new EnemyInfo(1, 10, 1.5f, 0, 4/*,sp1*/);

    //������һ�������Ĺ�����Ϣ

    //enemy_Middle
    static EnemyInfo enemy_2 = new EnemyInfo(2, 20, 1f, 0, 2);

    //enemy_Huge
    static EnemyInfo enemy_3 = new EnemyInfo(8, 64,0.8f, 0, 1);

    //enemy_Creeper
    static List<float> sp4 = new List<float>() { 25f, 2, 0.5f };//��ը�˺�,��ը��Χ,��ըǰҡ
    static EnemyInfo enemy_4 = new EnemyInfo(2, 30, 0.7f, 0, 2, sp4);

    //enemy_Skeleton
    //�����Ϲ���ʱӦ�û�ͣ����,����Ŀǰû�й�������͹�������ʱ�������,�����㶨һ����������ʱ��
    static List<float> sp5 = new List<float>() { 0.2f };//��������ʱ��
    static EnemyInfo enemy_5 = new EnemyInfo(3, 15, 1f, 4, 2, sp5);

    //enemy_Witch
    static List<float> sp6 = new List<float>() { 0.2f, 10, 0.2f, 10f };//��������ʱ��,�ٻ�����������,�ٻ������������,����һ���ٻ���������
    static EnemyInfo enemy_6 = new EnemyInfo(10, 200, 0.5f, 3.5f, 1, 110, sp6);

    //enemy_Creation
    //Witch������
    static EnemyInfo enemy_7 = new EnemyInfo(1, 5, 1.5f, 0, 5);

    //enemy_Guard
    static EnemyInfo enemy_8 = new EnemyInfo(2, 128, 0.7f, 0, 1, 105);

    //enemy_Shell
    static List<float> sp9 = new List<float>() {150f};//�ٻ��ϰ���Ѫ��
    static EnemyInfo enemy_9 = new EnemyInfo(2, 20, 1f, 0, 2, sp9);

    //enemy_Box
    static EnemyInfo enemy_10 = new EnemyInfo(0, 150, 1f, 0, 0);

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
            {enemyName[9],enemy_10},
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

    public static float InfinityStrategy(int extraTurn)
    {
        //�޾�ģʽ���Ѷ�����

        if (extraTurn <= 0) return 1;
        int count = (extraTurn - 1) % 5 + 1;

        float level = 1 + 0.3f * count;

        return level;
    }

}

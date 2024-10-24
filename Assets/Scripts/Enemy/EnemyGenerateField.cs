using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class EnemyGenerateField : MonoBehaviour
{
    //出生点第一个路径点
    [SerializeField]
    public RoadPoint StartPos;

    private List<string[]> levelTable = new List<string[]>();
    private PolygonCollider2D polygon;

    private float TurnCD = 0;

    private List<string> enemyName = new List<string>();
    private List<float> maxEnemyGenerateCD = new List<float>();
    private List<float> enemyGenerateCD = new List<float>();


    [SerializeField]
    private int turnDelay = 0;
    private List<int> roundInfinity = new List<int>() { 2,4,6,7,10 };


    void Start()
    {
        polygon = gameObject.GetComponent<PolygonCollider2D>();
    }
    void Update()
    {
        if (TurnCD > 0)
        {
            TurnCD -= Time.deltaTime;
            for (int i = 0; i < enemyGenerateCD.Count; i++)
            {
                enemyGenerateCD[i] -= Time.deltaTime;
                if (enemyGenerateCD[i] < 0)
                {
                    enemyGenerateCD[i] = maxEnemyGenerateCD[i];
                    GenerateEnemy(enemyName[i]);
                }
            }
        }
    }

    public void StartTurn(int turn)
    {
        if (levelTable == null)
        {
            Debug.LogError("没有波次表无法运行");
            return;
        }
        enemyName.Clear();
        maxEnemyGenerateCD.Clear();
        enemyGenerateCD.Clear();
        int realTurn = GetTurn(turn);

        if (realTurn <= 0)
        {
            //没到刷怪时间
            return;
        }


        if (levelTable[realTurn - 1].Length <= 2)
        {
            //不刷怪
        }
        else
        {
            TurnCD = GetTurnTime(turn);
            //感觉可以优化，但脑子有点转不动了
            for (int i = 2; i < levelTable[realTurn - 1].Length; i++){
                string[] sp = levelTable[realTurn - 1][i].Split('*');
                //Debug.Log(sp[0] + "  " + sp[1]);
                enemyName.Add(sp[0]);
                maxEnemyGenerateCD.Add(TurnCD / (float.Parse(sp[1]) * GetLevel(turn - turnDelay)));//随着难度改变会增加每波的怪物总量
                //立刻出怪
                //float cd = maxEnemyGenerateCD[i - 2];
                float cd = 0;
                enemyGenerateCD.Add(cd);
            }
        }
    }

    public void GetLevelTable(string name)
    {
        levelTable = LevelParser.ParsePSV(name);
    }

    public int GetTurnCount()
    {
        if (levelTable == null) return 0;
        return levelTable.Count;
    }

    public float GetTurnTime(int turn)
    {
        //没到刷怪时间
        if (levelTable == null) return 0;
        if (GetTurn(turn) == 0) return 0;

        return float.Parse(levelTable[GetTurn(turn) - 1][1]);
    }
    private void GenerateEnemy(string name)
    {
        GameObject enemy = EnemyPool.instance.GetEnemyFromPool(name);
        enemy.gameObject.transform.position = GetRandomPointInPolygonCollider();
        enemy.GetComponent<Enemy>().SetMap(StartPos);
        EnemyEventSystem.instance.EnemyGenerate(enemy.GetComponent<Enemy>());
    }

    private int GetTurn(int turn)
    {
        turn -= turnDelay;
        if (levelTable == null) return 0;
        if (turn <= 0) return 0;
        int count = levelTable.Count;
        if (count >= turn)
        {
            return turn;
        }
        else
        {
            //Debug.Log("获取的轮数高于最高轮数，进入无限模式");
            if (count < roundInfinity[roundInfinity.Count - 1])
            {
                Debug.LogError("总轮数少于设定循环的最高轮，无法构建循环");
                return 0;
            }
            else
            {
                //构建循环
                Debug.Log($"{turnDelay}infinity{turn}");
                return roundInfinity[(turn - 1 - count) % roundInfinity.Count];
            }
        }
    }

    private Vector3 GetRandomPointInPolygonCollider()
    {
        // 获取边界
        Bounds bounds = polygon.bounds;

        // 重复尝试找到一个在碰撞体内部的随机点
        Vector3 randomPoint;
        do
        {
            float x = Random.Range(bounds.min.x, bounds.max.x);
            float y = Random.Range(bounds.min.y, bounds.max.y);
            randomPoint = new Vector3(x, y, 0);
        } while (!polygon.OverlapPoint(randomPoint));

        return randomPoint;
    }


    private float GetLevel(int turn)
    {
        turn -= GetTurnCount();
        return EnemyData.InfinityStrategy(turn);
    }
}   

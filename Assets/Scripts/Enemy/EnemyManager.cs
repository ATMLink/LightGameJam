using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{


    //分四个刷怪点用于刷怪
    [SerializeField]
    private List<EnemyGenerateField> fieldList = new List<EnemyGenerateField>();

    //存储所有在场的enemy,暂时没用，可能用于动态调控难度
    private List<Enemy> enemyList = new List<Enemy>();

    //难度管理
    [SerializeField]
    private float difficultLevel = 1.0f;
    //所有出怪点开始无尽的回合数
    private int maxTurnNum = 0;

    //游戏时间进程管理

    enum GameState
    {
        None,//未开始
        inTurn,
        outTurn,
        pause,//暂时不写，用于游戏暂停
        end//结束
    }
    GameState state = GameState.inTurn;

    [SerializeField]
    private float maxRestCD = 30;//最大波次间隔时间
    private float restCD = 30;
    private bool trigger = false;
    //当前回合
    private int currentTurn = 0;
    private float currentTurnCD = 0;

    private void Awake()
    {
        SetLevelTable(1, "Level1");
    }

    private void Start()
    {
        EnemyEventSystem.instance.onGameStarting += GameStart;
        EnemyEventSystem.instance.onGameEnding += GameEnd;
        EnemyEventSystem.instance.onTurnNext += NextTurn;
        EnemyEventSystem.instance.onEnemyGenerating += EnemyGenerate;
        EnemyEventSystem.instance.onEnemyDestory += EnemyDestory;
    }

    private void Update()
    {
        switch (state)
        {
            case GameState.None:
                if (trigger == true) state = GameState.inTurn;
                break;
            case GameState.inTurn:
                if (trigger == true)
                {
                    SetDifficultLevel();
                    currentTurn += 1;
                    //获取所有场地刷新时间中最长的那个作为波次的持续时间
                    float maxTurnCD = 0;
                    foreach (var field in fieldList)
                    {
                        if (maxTurnCD < field.GetTurnTime(currentTurn))
                        {
                            maxTurnCD = field.GetTurnTime(currentTurn);
                        }
                    }
                    currentTurnCD = maxTurnCD;
                    //开始所有已绑定的刷怪点的刷怪行为
                    foreach (var field in fieldList)
                    {
                        field.StartTurn(currentTurn, difficultLevel);
                    }
                    trigger = false;
                }
                if (currentTurnCD > 0)
                {
                    currentTurnCD -= Time.deltaTime;
                }
                else
                {
                    //进入波次间隔
                    restCD = maxRestCD / difficultLevel;
                    state = GameState.outTurn;
                }
                break;
            case GameState.outTurn:

                if (restCD > 0)
                {
                    restCD -= Time.deltaTime;
                }
                else
                {
                    //进入下一波
                    state = GameState.inTurn;
                    trigger = true;
                }

                break;
            case GameState.pause:

                //需要实现：暂停所有怪物

                break;
            case GameState.end:
                //停止所有刷怪行为
                break;
        }
    }

    public void Generate()
    {
        if (state == GameState.None) GameStart();
        if (state == GameState.outTurn) NextTurn();
    }

    public void SetLevelTable(int count,string levelName)
    {
        fieldList[count - 1].GetLevelTable(levelName);
    }

    public float GetTurnTimeRemain()
    {
        return restCD / difficultLevel;
    }


    private void SetDifficultLevel()
    {
        //未启用
        //difficultLevel = EnemyData.InfinityStrategy(currentTurn - maxTurnNum);
    }

    private void GameStart()
    {
        trigger = true;
        maxTurnNum = 0;
        foreach (var field in fieldList)
        {
            if(maxTurnNum < field.GetTurnCount())
            {
                maxTurnNum = field.GetTurnCount();
            }
        }
    }

    private void GameEnd()
    {
        state = GameState.end;
    }
    private void NextTurn()
    {
        trigger = true;
        state = GameState.inTurn;
    }

    private void EnemyGenerate(Enemy enemy)
    {
        enemyList.Add(enemy);
    }

    private void EnemyDestory(Enemy enemy,string name = null)
    {
        if (enemyList.Contains(enemy))
        {
            //无序表尝逝一下优化
            enemyList[enemyList.IndexOf(enemy)] = enemyList[enemyList.Count - 1];
            enemyList.RemoveAt(enemyList.Count - 1);
        }
    }
}

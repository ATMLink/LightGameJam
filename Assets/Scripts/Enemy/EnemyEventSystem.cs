using System;
using UnityEngine;

public class EnemyEventSystem : MonoBehaviour
{

    public static EnemyEventSystem instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //游戏开始
    public event Action onGameStarting;
    public void GameStart()
    {
        if (onGameStarting != null)
        {
            onGameStarting();
        }
    }

    //游戏结束
    public event Action onGameEnding;
    public void GameEnd()
    {
        if (onGameEnding != null)
        {
            onGameEnding();
        }
    }

    //快进到下一波
    public event Action onTurnNext;
    public void TurnNext()
    {
        if (onTurnNext != null)
        {
            onTurnNext();
        }
    }

    //怪物生成
    public event Action<Enemy> onEnemyGenerating;
    public void EnemyGenerate(Enemy enemy)
    {
        if (onEnemyGenerating != null)
        {
            onEnemyGenerating(enemy);
        }
    }

    //怪物死亡
    public event Action<Enemy,string> onEnemyDestory;
    public void EnemyDestroy(Enemy enemy, string name = null)
    {
        if (onEnemyDestory != null)
        {
            onEnemyDestory(enemy, name);
        }
    }
}

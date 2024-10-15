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

    //��Ϸ��ʼ
    public event Action onGameStarting;
    public void GameStart()
    {
        if (onGameStarting != null)
        {
            onGameStarting();
        }
    }

    //��Ϸ����
    public event Action onGameEnding;
    public void GameEnd()
    {
        if (onGameEnding != null)
        {
            onGameEnding();
        }
    }

    //�������һ��
    public event Action onTurnNext;
    public void TurnNext()
    {
        if (onTurnNext != null)
        {
            onTurnNext();
        }
    }

    //��������
    public event Action<Enemy> onEnemyGenerating;
    public void EnemyGenerate(Enemy enemy)
    {
        if (onEnemyGenerating != null)
        {
            onEnemyGenerating(enemy);
        }
    }

    //��������
    public event Action<Enemy,string> onEnemyDestory;
    public void EnemyDestroy(Enemy enemy, string name = null)
    {
        if (onEnemyDestory != null)
        {
            onEnemyDestory(enemy, name);
        }
    }
}

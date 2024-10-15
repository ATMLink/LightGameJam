using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{


    //���ĸ�ˢ�ֵ�����ˢ��
    [SerializeField]
    private List<EnemyGenerateField> fieldList = new List<EnemyGenerateField>();

    //�洢�����ڳ���enemy,��ʱû�ã��������ڶ�̬�����Ѷ�
    private List<Enemy> enemyList = new List<Enemy>();

    //�Ѷȹ���
    [SerializeField]
    private float difficultLevel = 1.0f;
    //���г��ֵ㿪ʼ�޾��Ļغ���
    private int maxTurnNum = 0;

    //��Ϸʱ����̹���

    enum GameState
    {
        None,//δ��ʼ
        inTurn,
        outTurn,
        pause,//��ʱ��д��������Ϸ��ͣ
        end//����
    }
    GameState state = GameState.inTurn;

    [SerializeField]
    private float maxRestCD = 30;//��󲨴μ��ʱ��
    private float restCD = 30;
    private bool trigger = false;
    //��ǰ�غ�
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
                    //��ȡ���г���ˢ��ʱ��������Ǹ���Ϊ���εĳ���ʱ��
                    float maxTurnCD = 0;
                    foreach (var field in fieldList)
                    {
                        if (maxTurnCD < field.GetTurnTime(currentTurn))
                        {
                            maxTurnCD = field.GetTurnTime(currentTurn);
                        }
                    }
                    currentTurnCD = maxTurnCD;
                    //��ʼ�����Ѱ󶨵�ˢ�ֵ��ˢ����Ϊ
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
                    //���벨�μ��
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
                    //������һ��
                    state = GameState.inTurn;
                    trigger = true;
                }

                break;
            case GameState.pause:

                //��Ҫʵ�֣���ͣ���й���

                break;
            case GameState.end:
                //ֹͣ����ˢ����Ϊ
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
        //δ����
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
            //�������һ���Ż�
            enemyList[enemyList.IndexOf(enemy)] = enemyList[enemyList.Count - 1];
            enemyList.RemoveAt(enemyList.Count - 1);
        }
    }
}

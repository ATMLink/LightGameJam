using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyGenerateField : MonoBehaviour
{
    //�������һ��·����
    [SerializeField]
    public RoadPoint StartPos;

    private List<string[]> levelTable = new List<string[]>();
    private PolygonCollider2D polygon;

    private bool trigger = false;
    private float TurnCD = 0;

    private List<string> enemyName = new List<string>();
    private List<float> maxEnemyGenerateCD = new List<float>();
    private List<float> enemyGenerateCD = new List<float>();

    void Start()
    {
        polygon = gameObject.GetComponent<PolygonCollider2D>();
    }
    void Update()
    {
        if(trigger == true)
        {
            trigger = false;
        }
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

    public void StartTurn(int turn,float level)
    {
        if (levelTable == null)
        {
            Debug.LogError("û�в��α��޷�����");
            return;
        }
        enemyName.Clear();
        maxEnemyGenerateCD.Clear();
        enemyGenerateCD.Clear();
        int realTurn = GetTurn(turn);

        if (levelTable[realTurn - 1].Length <= 2)
        {
            //��ˢ��
        }
        else
        {
            TurnCD = GetTurnTime(turn);
            //�о������Ż����������е�ת������
            for (int i = 2; i < levelTable[realTurn - 1].Length; i++){
                string[] sp = levelTable[realTurn - 1][i].Split('*');
                Debug.Log(sp[0] + "  " + sp[1]);
                enemyName.Add(sp[0]);
                maxEnemyGenerateCD.Add(TurnCD / (float.Parse(sp[1]) * level));//�����Ѷȸı������ÿ���Ĺ�������
                //���̳���
                //float cd = maxEnemyGenerateCD[i - 2];
                float cd = 0;
                enemyGenerateCD.Add(cd);
            }
        }
        trigger = true;
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
        if (levelTable == null) return 0;
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
        //Ĭ���Ժ�10��Ϊѭ����
        if (levelTable == null) return 0;
        int count = levelTable.Count;
        if (count >= turn)
        {
            return turn;
        }
        else
        {
            Debug.Log("��ȡ���������������������������ģʽ");
            if (count < 10)
            {
                Debug.LogError("����������10�֣��޷�����ѭ��");
                return 0;
            }
            else
            {
                return count - 10 + (turn - count) % 10;
            }
        }
    }

    private Vector3 GetRandomPointInPolygonCollider()
    {
        // ��ȡ�߽�
        Bounds bounds = polygon.bounds;

        // �ظ������ҵ�һ������ײ���ڲ��������
        Vector3 randomPoint;
        do
        {
            float x = Random.Range(bounds.min.x, bounds.max.x);
            float y = Random.Range(bounds.min.y, bounds.max.y);
            randomPoint = new Vector3(x, y, 0);
        } while (!polygon.OverlapPoint(randomPoint));

        return randomPoint;
    }
}   

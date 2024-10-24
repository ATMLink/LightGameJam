using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool instance;

    [SerializeField]
    private List<GameObject> enemyPrefabList = new List<GameObject>();
    [SerializeField]
    private int originPoolSize = 5;

    private void Awake()
    {
        if (instance == null)instance = this;
        else Destroy(gameObject);
    }



    private Dictionary<string, Queue<Enemy>> enemyPoolDic = new Dictionary<string, Queue<Enemy>>()
    {
            {EnemyData.enemyName[0],new Queue<Enemy>()},
            {EnemyData.enemyName[1],new Queue<Enemy>()},
            {EnemyData.enemyName[2],new Queue<Enemy>()},
            {EnemyData.enemyName[3],new Queue<Enemy>()},
            {EnemyData.enemyName[4],new Queue<Enemy>()},
            {EnemyData.enemyName[5],new Queue<Enemy>()},
            {EnemyData.enemyName[6],new Queue<Enemy>()},
            {EnemyData.enemyName[7],new Queue<Enemy>()},
            {EnemyData.enemyName[8],new Queue<Enemy>()},
            {EnemyData.enemyName[9],new Queue<Enemy>()},
    };

    private void Start()
    {
        EnemyEventSystem.instance.onEnemyDestory += ReturnEnemyToPool;

        for(int i = 0; i < enemyPoolDic.Count; i++)
        {
            GeneratePool(EnemyData.enemyName[i]);
        }
    }



    public GameObject GetEnemyFromPool(string name)
    {
        // �Ӷ�����л�ȡ����
        if (enemyPoolDic.ContainsKey(name))
        {
            if(enemyPoolDic[name].Count > 0)
            {
                GameObject obj = enemyPoolDic[name].Dequeue().gameObject;
                if (obj == null)
                {
                    //���Ԥ�����ѱ��ݻ���ݹ����
                    return GetEnemyFromPool(name);
                }
                obj.SetActive(true);

                //��ǰ������һ��
                if (enemyPoolDic[name].Count == 0)
                {
                    GameObject newObj = Instantiate(enemyPrefabList[EnemyData.enemyName.IndexOf(name)], transform.position, Quaternion.identity);
                    enemyPoolDic[name].Enqueue(newObj.GetComponent<Enemy>());
                    newObj.SetActive(false);
                }

                return obj;
            }
            else
            {
                // ����������û�п��ö����򴴽��µĶ��󲢷���

                GameObject newObj = Instantiate(enemyPrefabList[EnemyData.enemyName.IndexOf(name)], transform.position, Quaternion.identity);
                return newObj;
            }

        }
        else
        {
            Debug.LogError("������в����ڸ�����");
            return null;
        }
    }


    //private IEnumerator generation(string name)
    //{
    //    yield break;
    //}

    public void ReturnEnemyToPool(Enemy enemy,string name = null)
    {
        //�������ַ�ʽ���ض����
        if (name == null)
        {
            if (enemy == null)
            {
                Debug.LogError("�����ڸö����");
                return;
            }
            name = enemy.GetEnemyName();
        }

        if (EnemyData.enemyName.Contains(name))
        {
            // ������Żض������
            if (!enemyPoolDic[name].Contains(enemy))
            {
                //��ֹһ�������ظ����
                enemyPoolDic[name].Enqueue(enemy);
                enemy.gameObject.transform.SetParent(transform);
                enemy.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.LogError("������в����ڸ�����");
            Destroy(enemy.gameObject);
        }
    }

    private void GeneratePool(string name)
    {
        for (int i = 0; i < originPoolSize; i++)
        {
            GameObject newObj = Instantiate(enemyPrefabList[EnemyData.enemyName.IndexOf(name)], transform.position, Quaternion.identity);
            enemyPoolDic[name].Enqueue(newObj.GetComponent<Enemy>());
            newObj.transform.SetParent(transform);
            newObj.SetActive(false);
            //StartCoroutine(generation(name));
        }
    }
}

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
        // 从对象池中获取对象
        if (enemyPoolDic.ContainsKey(name))
        {
            if(enemyPoolDic[name].Count > 0)
            {
                GameObject obj = enemyPoolDic[name].Dequeue().gameObject;
                if (obj == null)
                {
                    //如果预制体已被摧毁则递归调用
                    return GetEnemyFromPool(name);
                }
                obj.SetActive(true);

                //提前创建下一个
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
                // 如果对象池中没有可用对象，则创建新的对象并返回

                GameObject newObj = Instantiate(enemyPrefabList[EnemyData.enemyName.IndexOf(name)], transform.position, Quaternion.identity);
                return newObj;
            }

        }
        else
        {
            Debug.LogError("对象池中不存在该名称");
            return null;
        }
    }


    //private IEnumerator generation(string name)
    //{
    //    yield break;
    //}

    public void ReturnEnemyToPool(Enemy enemy,string name = null)
    {
        //允许两种方式返回对象池
        if (name == null)
        {
            if (enemy == null)
            {
                Debug.LogError("不存在该对象池");
                return;
            }
            name = enemy.GetEnemyName();
        }

        if (EnemyData.enemyName.Contains(name))
        {
            // 将对象放回对象池中
            if (!enemyPoolDic[name].Contains(enemy))
            {
                //防止一个物体重复入队
                enemyPoolDic[name].Enqueue(enemy);
                enemy.gameObject.transform.SetParent(transform);
                enemy.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.LogError("对象池中不存在该名称");
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

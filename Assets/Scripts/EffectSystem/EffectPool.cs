using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPool : MonoBehaviour
{
    public static EffectPool instance;

    [SerializeField]
    private List<GameObject> EffectPrefab = new List<GameObject>();

    private List<string> EffectName = new List<string>()
    {
        "EnemyDeathEffect",
        "TowerPlaceEffect",
        "TowerDeathEffect",
        "NaBoomEffect",
    };

    public int originPoolSize = 5;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private Dictionary<string, Queue<Effect>> EffectDic = new Dictionary<string, Queue<Effect>>()
    {
            {"EnemyDeathEffect",new Queue<Effect>()},
            {"TowerPlaceEffect",new Queue<Effect>()},
            {"TowerDeathEffect",new Queue<Effect>()},
            {"NaBoomEffect",new Queue<Effect>()},
    };

    private void Start()
    {
        for (int i = 0; i < EffectDic.Count; i++)
        {
            GeneratePool(EffectName[i]);
        }
    }


    public Effect GetObjFromPool(string name)
    {
        // 从对象池中获取对象
        if (EffectDic.ContainsKey(name))
        {
            if (EffectDic[name].Count > 0)
            {
                Effect pro = EffectDic[name].Dequeue();
                if (pro == null)
                {
                    //如果预制体已被摧毁则递归调用
                    return GetObjFromPool(name);
                }
                pro.gameObject.SetActive(true);

                //提前创建下一个
                if (EffectDic[name].Count == 0)
                {
                    GameObject newObj = Instantiate(EffectPrefab[EffectName.IndexOf(name)], transform.position, Quaternion.identity);
                    EffectDic[name].Enqueue(newObj.GetComponent<Effect>());
                    newObj.SetActive(false);
                }

                return pro;
            }
            else
            {
                // 如果对象池中没有可用对象，则创建新的对象并返回

                GameObject newObj = Instantiate(EffectPrefab[EffectName.IndexOf(name)], transform.position, Quaternion.identity);
                return newObj.GetComponent<Effect>();
            }

        }
        else
        {
            Debug.LogError("对象池中不存在该名称");
            return null;
        }
    }


    public void ReturnObjToPool(Effect pro, string name)
    {

        if (EffectDic.ContainsKey(name))
        {
            // 将对象放回对象池中
            if (!EffectDic[name].Contains(pro))
            {
                //防止一个物体重复入队
                EffectDic[name].Enqueue(pro);
                pro.gameObject.transform.SetParent(transform);
                pro.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.LogError("对象池中不存在该名称");
            Destroy(pro.gameObject);
        }
    }

    private void GeneratePool(string name)
    {
        for (int i = 0; i < originPoolSize; i++)
        {
            GameObject newObj = Instantiate(EffectPrefab[EffectName.IndexOf(name)], transform.position, Quaternion.identity);
            EffectDic[name].Enqueue(newObj.GetComponent<Effect>());
            newObj.transform.SetParent(transform);
            newObj.SetActive(false);
        }
    }

}

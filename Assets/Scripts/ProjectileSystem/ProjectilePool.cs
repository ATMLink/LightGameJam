using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    public static ProjectilePool instance;

    [SerializeField]
    private List<GameObject> ProjectilePrefab = new List<GameObject>();

    [SerializeField]
    private List<string> ProjectileName = new List<string>()
    {
        "TowerProjectile",
        "EnemyProjectile",
        "KProjectile",
        "NaProjectile",
    };

    public int originPoolSize = 5;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private Dictionary<string, Queue<Projectile>> ProjectileDic = new Dictionary<string, Queue<Projectile>>()
    {
            {"TowerProjectile",new Queue<Projectile>()},
            {"EnemyProjectile",new Queue<Projectile>()},
            {"KProjectile",new Queue<Projectile>()},
            {"NaProjectile",new Queue<Projectile>()},
    };

    private void Start()
    {
        for (int i = 0; i < ProjectileDic.Count; i++)
        {
            GeneratePool(ProjectileName[i]);
        }
    }


    public Projectile GetObjFromPool(string name)
    {
        // 从对象池中获取对象
        if (ProjectileDic.ContainsKey(name))
        {
            if (ProjectileDic[name].Count > 0)
            {
                Projectile pro = ProjectileDic[name].Dequeue();
                if (pro == null)
                {
                    //如果预制体已被摧毁则递归调用
                    return GetObjFromPool(name);
                }
                pro.gameObject.SetActive(true);

                //提前创建下一个
                if (ProjectileDic[name].Count == 0)
                {
                    GameObject newObj = Instantiate(ProjectilePrefab[ProjectileName.IndexOf(name)], transform.position, Quaternion.identity);
                    ProjectileDic[name].Enqueue(newObj.GetComponent<Projectile>());
                    newObj.SetActive(false);
                }

                return pro;
            }
            else
            {
                // 如果对象池中没有可用对象，则创建新的对象并返回

                GameObject newObj = Instantiate(ProjectilePrefab[ProjectileName.IndexOf(name)], transform.position, Quaternion.identity);
                return newObj.GetComponent<Projectile>();
            }

        }
        else
        {
            Debug.LogError("对象池中不存在该名称");
            return null;
        }
    }


    public void ReturnObjToPool(Projectile pro, string name)
    {

        if (ProjectileDic.ContainsKey(name))
        {
            // 将对象放回对象池中
            if (!ProjectileDic[name].Contains(pro))
            {
                //防止一个物体重复入队
                ProjectileDic[name].Enqueue(pro);
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
            GameObject newObj = Instantiate(ProjectilePrefab[ProjectileName.IndexOf(name)], transform.position, Quaternion.identity);
            ProjectileDic[name].Enqueue(newObj.GetComponent<Projectile>());
            newObj.transform.SetParent(transform);
            newObj.SetActive(false);
        }
    }



}

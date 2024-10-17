using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPool : MonoBehaviour
{
    public Tower towerPrefab; // 塔的预制件
    private List<Tower> pool = new List<Tower>();
    private int initialCapacity = 10; // 初始容量

    void Start()
    {
        // 初始化对象池
        for (int i = 0; i < initialCapacity; i++)
        {
            CreateNewTower();
        }
    }

    private Tower CreateNewTower()
    {
        Tower newTower = Instantiate(towerPrefab);
        newTower.gameObject.SetActive(false);
        pool.Add(newTower);
        return newTower;
    }

    public Tower GetTower()
    {
        foreach (Tower tower in pool)
        {
            if (!tower.gameObject.activeInHierarchy)
            {
                tower.gameObject.SetActive(true);
                return tower;
            }
        }
        return CreateNewTower(); // 如果池中没有可用对象，则扩容
    }

    public void ReturnTower(Tower tower)
    {
        tower.gameObject.SetActive(false); // 将塔标记为不可见
    }
}

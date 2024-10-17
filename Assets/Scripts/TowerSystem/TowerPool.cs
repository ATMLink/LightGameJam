using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPool : MonoBehaviour
{
    public Tower towerPrefab;
    private List<Tower> pool = new List<Tower>();
    private int initialCapacity = 10; // 初始容量
    private int maxCapacity = 50;     // 最大容量

    public void Initialize()
    {
        // 初始化对象池
        for (int i = 0; i < initialCapacity; i++)
        {
            CreateNewTower();
        }
    }

    private Tower CreateNewTower()
    {
        if (pool.Count >= maxCapacity)
        {
            Debug.LogWarning("TowerPool has reached its maximum capacity.");
            return null;
        }

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
        tower.ResetAttributes(); // 重置塔的属性
        tower.gameObject.SetActive(false); // 将塔标记为不可见
    }
}

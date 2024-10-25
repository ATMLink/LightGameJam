using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TowerPool : MonoBehaviour
{
    // 用于存储不同类型塔的Prefab
    [SerializeField] private List<Tower> towerPrefabs;  // 将不同的塔预设存储在列表中
    private Dictionary<Tower, List<Tower>> poolDictionary = new Dictionary<Tower, List<Tower>>();

    private int initialCapacity = 10; // 初始容量

    public void Initialize()
    {
        // 为每种塔类型初始化对象池
        foreach (Tower towerPrefab in towerPrefabs)
        {
            poolDictionary[towerPrefab] = new List<Tower>();

            for (int i = 0; i < initialCapacity; i++)
            {
                CreateNewTower(towerPrefab);
            }
        }
    }

    private Tower CreateNewTower(Tower towerPrefab)
    {

        Tower newTower = Instantiate(towerPrefab);
        newTower.gameObject.SetActive(false);
        poolDictionary[towerPrefab].Add(newTower);
        return newTower;
    }

    // 获取特定类型的塔
    public Tower GetTower(Tower towerPrefab)
    {
        if (!poolDictionary.ContainsKey(towerPrefab))
        {
            Debug.LogWarning($"No pool exists for {towerPrefab.name}. Creating a new pool.");
            poolDictionary[towerPrefab] = new List<Tower>();
        }

        foreach (Tower tower in poolDictionary[towerPrefab])
        {
            if (!tower.gameObject.activeInHierarchy)
            {
                tower.gameObject.SetActive(true);
                return tower;
            }
        }

        return CreateNewTower(towerPrefab); // 如果池中没有可用对象，则扩容
    }

    public void ReturnTower(Tower tower)
    {
        tower.ResetAttributes(); // 重置塔的属性
        tower.gameObject.SetActive(false); // 将塔标记为不可见
    }
}

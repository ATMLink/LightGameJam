using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructManager : MonoBehaviour
{
    [SerializeField] private TowerPool towerPool;
    private TowerAttributes selectedTowerAttributes;

    // 设置当前选择的塔
    public void SelectTower(TowerAttributes towerAttributes)
    {
        selectedTowerAttributes = towerAttributes;
    }

    public void PlaceTower(Vector3 position)
    {
        Debug.Log($"Placing tower at position: {position}");
        if (selectedTowerAttributes != null)
        {
            if (CanPlaceTower(position)) // 检查是否可以放置塔
            {
                Tower newTower = towerPool.GetTower();
                if (newTower != null) // 确保池子未满
                {
                    newTower.transform.position = position;
                    newTower.attributes = selectedTowerAttributes;
                    newTower.Initialize();
                }
            }
            else
            {
                Debug.Log("无法在此位置放置塔。");
            }
        }
    }

    // 检查指定位置是否允许放置塔
    private bool CanPlaceTower(Vector3 position)
    {
        // 检查是否已有塔
        Tower existingTower = FindObjectOfType<TowerManager>().GetTowerAt(position);
        if (existingTower != null)
        {
            return false; // 已有塔，不能放置
        }

        // 可以添加其他条件检查

        return true; // 可以放置
    }
    
}

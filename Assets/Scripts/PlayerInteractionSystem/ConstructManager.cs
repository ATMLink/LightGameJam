using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ConstructManager : MonoBehaviour
{
    [SerializeField] private TowerManager towerManager;
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
            if (CanPlaceTower(selectedTowerAttributes,position)) // 检查是否可以放置塔
            {
                towerManager.AddTower(position, selectedTowerAttributes);
                // Tower newTower = towerPool.GetTower();
                // if (newTower != null) // 确保池子未满
                // {
                //     // newTower.transform.position = position;
                //     // newTower.attributes = selectedTowerAttributes;
                //     // newTower.Initialize();
                //     towerManager.AddTower(position, selectedTowerAttributes);
                // }
            }
            else
            {
                Debug.Log("无法在此位置放置塔。");
            }
        }
    }

    // 检查指定位置是否允许放置塔
    private bool CanPlaceTower(TowerAttributes towerAttributes,Vector3 position)
    {
        // 检查是否已有塔
        Tower existingTower = towerManager.GetTowerAt(position);
        if (existingTower != null)
            return false; // 已有塔，不能放置
        else
        {
            Debug.Log("null");
        }
        // detect is normal tile or not
        float radius = 0.25f;
        TilemapFeature temp;
        Collider2D collider = Physics2D.OverlapCircle(position, radius, 1);
        if (collider != null)
        {
            GameObject foundObject = collider.gameObject;
            if (foundObject != null)
            {
                temp = foundObject.GetComponent<TilemapFeature>();
                if (!temp.canConstruct)
                {
                    Debug.Log(towerAttributes.name);
                    if (towerAttributes.name=="Basic" && temp.canMinerConstruct)return true;
                    return false;
                }
            }
        }
        return true; // 可以放置
    }
        
        
    
}

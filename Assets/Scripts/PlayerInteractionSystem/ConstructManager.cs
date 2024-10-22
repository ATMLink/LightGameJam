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
        //bool canConstruct = false;
        int count = 0;
        float radius = 0f;
        TilemapFeature temp;
        Collider2D[] collider = Physics2D.OverlapCircleAll(position, radius);
        if (collider.Length == 1) { return (towerAttributes.name == "Miner")?false:true; }
        else foreach (Collider2D col in collider)
        {

                GameObject foundObject = col.gameObject;
                Debug.LogWarning(foundObject.transform.position);
                if (foundObject.tag == "Tilemap") continue;
                else if (foundObject.tag == "Tile")
                {
                    temp = foundObject.GetComponent<TilemapFeature>();
                    if (towerAttributes.name == "Miner" && temp.canMinerConstruct) return true;
                    if (!temp.canConstruct)
                    {
                        Debug.LogWarning(towerAttributes.name);
                        Debug.LogWarning(1);
                        return false;
                    }
                }
            }
            else if (foundObject.tag == "Tower")
            {
                if (foundObject.transform.position == position)
                {
                    if (foundObject.transform.position == position)
                    {
                        count++;
                    }
                    if (count == 1) { Debug.LogWarning(2); return false; }
                }
            }

        }
        return true; // 可以放置
    }
    // private bool CanPlaceTower(TowerAttributes towerAttributes,Vector3 position)
    // {
    //     int count = 0;
    //     float radius = 0.25f;
    //     TilemapFeature temp;
    //     Collider2D[] collider = Physics2D.OverlapCircleAll(position, radius);
    //     Debug.LogWarning(collider);
    //     if (collider.Length == 1) return true;
    //     else foreach (Collider2D col in collider)
    //         {
    //
    //             GameObject foundObject = col.gameObject;
    //             if (foundObject.tag == "Tilemap") continue;
    //             else if (foundObject.tag == "Tile")
    //             {
    //                 temp = foundObject.GetComponent<TilemapFeature>();
    //                 if (!temp.canConstruct)
    //                 {
    //                     Debug.Log(towerAttributes.name);
    //                     if (towerAttributes.name == "Basic" && temp.canMinerConstruct) return true;
    //                     return false;
    //                 }
    //             }
    //             else if (foundObject.tag == "Tower")
    //             {
    //                 count++;
    //                 if (count == 2) return false;
    //             }
    //
    //         }
    //     return true; // 可以放置
    // }
        
    
    
}

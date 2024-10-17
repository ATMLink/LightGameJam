using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructManager : MonoBehaviour
{
    [SerializeField] private TowerPool towerPool;
    private TowerAttributes selectedTowerAttributes; // 当前选择的塔属性

    // 方法用于设置当前选择的塔
    public void SelectTower(TowerAttributes towerAttributes)
    {
        selectedTowerAttributes = towerAttributes;
    }

    public void PlaceTower(Vector3 position)
    {
        if (selectedTowerAttributes != null)
        {
            Tower newTower = towerPool.GetTower();
            newTower.transform.position = position;
            newTower.attributes = selectedTowerAttributes;
            newTower.Initialize();
        }
    }
}

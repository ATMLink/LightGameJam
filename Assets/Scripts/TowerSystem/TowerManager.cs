using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public TowerPool towerPool; // 对象池引用
    private List<Tower> towers = new List<Tower>(); // 当前场景中的塔

    // 添加塔
    public void AddTower(Vector3 position, TowerAttributes towerAttributes)
    {
        Tower newTower = towerPool.GetTower(); // 从对象池获取塔
        newTower.transform.position = position;
        newTower.attributes = towerAttributes; // 赋值塔的属性
        towers.Add(newTower); // 添加到管理列表
    }

    // 移除塔
    public void RemoveTower(Tower tower)
    {
        towerPool.ReturnTower(tower); // 将塔返回到对象池
        towers.Remove(tower); // 从列表中移除
    }

    // 获取指定位置的塔
    public Tower GetTowerAt(Vector3 position)
    {
        return towers.Find(tower => tower.transform.position == position);
    }
}

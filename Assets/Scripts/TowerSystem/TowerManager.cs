using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public List<Tower> towers = new List<Tower>();

    public void AddTower(Tower towerPrefab, Vector3 position)
    {
        Tower newTower = Instantiate(towerPrefab, position, Quaternion.identity);
        towers.Add(newTower);
        Debug.Log("Tower added at position: " + position);
    }

    public void RemoveTower(Tower tower)
    {
        towers.Remove(tower);
        tower.DestroyTower();
    }

    public Tower GetTowerAt(Vector3 position)
    {
        foreach (var tower in towers)
        {
            if (tower.transform.position == position)
            {
                return tower;
            }
        }
        return null;
    }
}

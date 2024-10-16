using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TowerManager : MonoBehaviour
{
    public TowerPool towerPool;
    private List<Tower> towers = new List<Tower>();

    public void Initialize()
    {
        towerPool.Initialize();
    }

    public void UpdateState()
    {
        // 批量更新塔的状态，例如攻击逻辑等
        foreach (Tower tower in towers)
        {
            // 执行塔的攻击逻辑或其他需要定期更新的操作
            tower.Attack();
        }
    }

    public void AddTower(Vector3 position, TowerAttributes towerAttributes)
    {
        Tower newTower = towerPool.GetTower();
        if (newTower != null) // 确保池子未满
        {
            newTower.transform.position = position;
            newTower.attributes = towerAttributes;
            towers.Add(newTower);
        }
    }

    public void UpgradeTower(Tower tower)
    {
        tower.Upgrade();
    }
    public void RotateTower(Tower tower, bool clockwise = true)
    {
        if (tower != null)
        {
            float angle = clockwise ? 90f : -90f;
            Vector3 targetRotation = tower.transform.eulerAngles + new Vector3(0, 0, angle);

            tower.transform.DORotate(targetRotation, 0.5f).SetEase(Ease.OutQuad);
        }
    }

    public void RemoveTower(Tower tower)
    {
        towerPool.ReturnTower(tower);
        towers.Remove(tower);
    }

    public Tower GetTowerAt(Vector3? position)
    {
        if (!position.HasValue)
            return null;
        return towers.Find(tower => tower.transform.position == position.Value);
    }
}

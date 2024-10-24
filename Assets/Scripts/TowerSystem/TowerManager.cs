using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using DG.Tweening;
using Vector3 = UnityEngine.Vector3;

public class TowerManager : MonoBehaviour
{
    public TowerPool towerPool;
    [SerializeField] private LaserManager laserManager;
    private List<Tower> towers = new List<Tower>();
    private bool isRotating = false;

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
            tower.UpdateState();
        }
    }

    public void AddTower(Vector3 position, TowerAttributes towerAttributes)
    {
        Tower newTower = towerPool.GetTower(towerAttributes.Prefab.GetComponent<Tower>());
        if (newTower != null) // 确保池子未满
        {
            newTower.transform.position = position;
            newTower.attributes = towerAttributes;
            towers.Add(newTower);
            newTower.Initialize();
            // create lasers
        }
    }

    public void UpgradeTower(Tower tower)
    {
        tower.Upgrade();
    }
    public void RotateTower(Tower tower, bool antiClockwise = true)
    {
        if (tower != null && !isRotating)
        {
            isRotating = true;  // 标记为旋转中

            StartRotate(tower);

            float angle = antiClockwise ? 45f : -45f;
            Vector3 targetRotation = tower.transform.eulerAngles + new Vector3(0, 0, angle);

            tower.transform.DORotate(targetRotation, 0.5f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => 
                {
                    EndRotate(tower, antiClockwise);
                    isRotating = false;  // 旋转结束后，允许新的旋转
                });  
        }
    }

    public void RemoveTower(Tower tower)
    {
        Debug.Log($"Removing tower: {tower.name}, at position: {tower.transform.position}");
        laserManager.RemoveLaser(tower);
        towerPool.ReturnTower(tower);
        towers.Remove(tower);
    }

    public Tower GetTowerAt(Vector3? position)
    {
        if (!position.HasValue)
            return null;
        // Debug.Log($"get {towers.Find(tower => IsPositionApproximatelyEqual(tower.transform.position, position.Value)).name}" +
        //           $" at {position.Value}");
        return towers.Find(tower => IsPositionApproximatelyEqual(tower.transform.position, position.Value));
    }
    private bool IsPositionApproximatelyEqual(Vector3 pos1, Vector3 pos2, float tolerance = 1f)
    {
        return Vector3.Distance(new Vector3(pos1.x, 0, pos1.z), new Vector3(pos2.x, 0, pos2.z)) < tolerance;
    }

    private void StartRotate(Tower tower)
    {
        laserManager.SetLaserActiveForTower(tower, false);
    }

    private void EndRotate(Tower tower, bool antiClockwise)
    {
        laserManager.SetLaserActiveForTower(tower, true);
        laserManager.RotateLaser(tower,antiClockwise);
    }
    
}

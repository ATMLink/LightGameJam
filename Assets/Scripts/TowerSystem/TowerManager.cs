using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using Vector3 = UnityEngine.Vector3;
using UnityEngine.UIElements;

public class TowerManager : MonoBehaviour
    {

    public TowerPool towerPool;
    [SerializeField] private LaserManager laserManager;
    [SerializeField] private MusicManager musicManager;

    //为了测试公开的
    [SerializeField]
    private List<Tower> towers = new List<Tower>();
    private bool isRotating = false;

    public TowerAttributes coreTower;//这里放要生成的核心塔
    // private void Start()
    // {
    //     laserManager.CreateLaser(testTower, testTower.transform.position, Vector3.down, 400);
    // }

    public void Initialize()
        {
        towerPool.Initialize();

        }

    IEnumerator CreatCoreTower()
        {
        yield return new WaitForSeconds(1);//等1秒
        Debug.Log("调用协程创建核心塔中");
        AddTower(new Vector3(-1.5f, 1.5f, 0), coreTower);

        }

    void Start()//场景加载完成调用
        {
        Debug.Log("调用协程创建核心塔开始");
        //创建核心塔
        StartCoroutine(CreatCoreTower());
        Debug.Log("调用协程创建核心塔成功");
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
            if (towerAttributes.towerName == "CoreTower_Lv1")
                {
                laserManager.CreateLaser(newTower, position, new Vector3(0, -1, 0), 400);
                Debug.Log("调用成功");
                }
            }
        }

    public void UpgradeTower(Tower tower)
        {
        tower.Upgrade();
        Debug.LogWarning(tower.name);
        if (tower.name == "Miner(Clone)")
            laserManager.CreateLaser(tower, tower.transform.position, Vector3.down, 300);
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
        if (musicManager != null)
            musicManager.PlaySound("DestructTower");
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
        if (tower.name == "SplitterTower")
            return;
        laserManager.SetLaserActiveForTower(tower, false);
        if (musicManager != null)
            musicManager.PlaySound("TowerRotate");
    }

    private void EndRotate(Tower tower, bool antiClockwise)
    {
        if (tower.name == "SplitterTower")
        {
            Debug.Log("splitter tower laser should not rotate");
            return;
        }
            
        laserManager.SetLaserActiveForTower(tower, true);
        laserManager.RotateLaser(tower, antiClockwise);
    }

    }

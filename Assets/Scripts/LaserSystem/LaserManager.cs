using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserManager : MonoBehaviour
{
    public LaserPool laserPool; // 激光池引用
    private Dictionary<Tower, Laser> towerLaserMap = new Dictionary<Tower, Laser>(); // 存储塔与激光的映射关系

    public void Initialize()
    {
        laserPool.Initialize();
    }

    public void UpdateState()
    {
        // 更新所有激活的激光状态
        foreach (Laser laser in towerLaserMap.Values)
        {
            laser.UpdateState();
        }
    }

    /// <summary>
    /// 创建激光绑定塔和激光
    /// </summary>
    /// <param name="position"></param>
    /// <param name="direction"></param>
    /// <param name="intensity"></param>
    /// <returns></returns>
    public Laser CreateLaser(Tower tower, Vector3 position, Vector3 direction, float intensity)
    {
        // 从对象池获取激光
        Laser laser = laserPool.GetLaser(position, direction, intensity);
        if (laser != null)
        {
            towerLaserMap[tower] = laser; // 绑定塔和激光
            laser.Initialize();
            laser.SetLaserActive(true);
        }
        return laser;
    }

    public void RemoveLaser(Tower tower)
    {
        if (towerLaserMap.ContainsKey(tower))
        {
            Laser laser = towerLaserMap[tower];
            laserPool.ReturnLaser(laser); // 将激光返回池中
            towerLaserMap.Remove(tower); // 从映射关系中移除激光
        }
    }
    
    /// <summary>
    /// 获取指定塔的激光
    /// </summary>
    public Laser GetLaserForTower(Tower tower)
    {
        if (towerLaserMap.ContainsKey(tower))
        {
            return towerLaserMap[tower];
        }
        return null;
    }
    
    /// <summary>
    /// 设置指定塔的激光的激活状态
    /// </summary>
    public void SetLaserActiveForTower(Tower tower, bool isActive)
    {
        Laser laser = GetLaserForTower(tower);
        if (laser != null)
        {
            laser.SetLaserActive(isActive);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserManager : MonoBehaviour
{
    public LaserPool laserPool; // 激光池引用
    private Dictionary<Tower, List<Laser>> towerLaserMap = new Dictionary<Tower, List<Laser>>(); // 存储塔与激光的映射关系

    public void Initialize()
    {
        laserPool.Initialize();
    }

    public void UpdateState()
    {
        // 更新所有激活的激光状态
        foreach (List<Laser> laserList in towerLaserMap.Values)
        {
            foreach (Laser laser in laserList)
            {
                laser.UpdateState();
            }
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
            if (!towerLaserMap.ContainsKey(tower))
                towerLaserMap[tower] = new List<Laser>();
            
            towerLaserMap[tower].Add(laser); // 绑定塔和激光
            laser.Initialize();
            laser.SetLaserActive(true);
        }
        return laser;
    }

    public void RemoveLaser(Tower tower)
    {
        if (towerLaserMap.ContainsKey(tower))
        {
            // 获取塔对应的激光列表
            List<Laser> laserList = towerLaserMap[tower];
        
            // 将所有激光返回到对象池中
            foreach (Laser laser in laserList)
            {
                laserPool.ReturnLaser(laser);
            }
        
            // 清空该塔的激光列表
            towerLaserMap.Remove(tower);
        }
    }
    
    /// <summary>
    /// 获取指定塔的激光
    /// </summary>
    public List<Laser> GetLaserForTower(Tower tower)
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
        List<Laser> lasers = GetLaserForTower(tower);
        if (lasers != null)
        {
            foreach (Laser laser in lasers)
            {
                laser.SetLaserActive(isActive);
            }
            
        }
    }

    public void RotateLaser(Tower tower, bool antiClockwise = true)
    {
        if (tower != null && towerLaserMap.ContainsKey(tower))
        {
            // 获取该塔对应的所有激光
            List<Laser> lasers = towerLaserMap[tower];

            // 旋转角度，逆时针为正，顺时针为负
            float angle = antiClockwise ? 45f : -45f;

            // 遍历所有激光，更新每个激光的方向
            foreach (Laser laser in lasers)
            {
                if (laser != null)
                {
                    // 获取激光当前的方向
                    Vector3 currentDirection = laser.direction;

                    // 计算新的旋转角度（绕Z轴旋转2D平面中的方向）
                    Vector3 newDirection = Quaternion.Euler(0, 0, angle) * currentDirection;

                    // 更新激光的方向
                    laser.UpdateLaserPositionAndDirection(laser.transform.position, newDirection);
                }
            }
        }
    }
}

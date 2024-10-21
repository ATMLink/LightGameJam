using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserManager : MonoBehaviour
{
    public LaserPool laserPool; // 激光池引用
    private List<Laser> activeLasers = new List<Laser>(); // 存储当前激活的激光

    public void Initialize()
    {
        laserPool.Initialize();
    }

    public void UpdateState()
    {
        // 更新所有激活的激光状态
        foreach (Laser laser in activeLasers)
        {
            laser.UpdateState();
        }
    }

    /// <summary>
    /// Create a laser with parameters position, direction, intensity
    /// </summary>
    /// <param name="position"></param>
    /// <param name="direction"></param>
    /// <param name="intensity"></param>
    /// <returns></returns>
    public Laser CreateLaser(Vector3 position, Vector3 direction, float intensity)
    {
        // 从对象池获取激光
        Laser laser = laserPool.GetLaser(position, direction, intensity);
        if (laser != null)
        {
            activeLasers.Add(laser);
            laser.Initialize();
            laser.SetLaserActive(true);
        }
        return laser;
    }

    public void RemoveLaser(Laser laser)
    {
        laserPool.ReturnLaser(laser); // 将激光返回池中
        activeLasers.Remove(laser); // 从激活列表中移除激光
    }
}

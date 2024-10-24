using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineTower : Tower
{
    public int maxLasers = 7; // 合光塔最多接受7个激光
    public Vector3 blockedDirection = Vector3.down; // 定义不能接受激光的方向（这里假设右侧不能接受）

    private float totalIntensity = 0;
    private float maxTotalIntensity = 200f; // 合光塔的最大激光强度
    private float updateInterval = 0.1f; // 每隔0.1秒更新一次激光强度
    private float lastUpdateTime = 0f;

    private LaserManager laserManager;
    private Vector3 outputDirection = Vector3.up; // 合光塔输出激光的方向
    
    public override void Initialize()
    {
        base.Initialize();
        laserManager = FindObjectOfType<LaserManager>();
        // 合光塔初始化不创建激光，等待接收到足够的输入激光后才开始输出
    }

    public override void UpdateState()
    {
        // 更新激光强度
        if (Time.time - lastUpdateTime >= updateInterval)
        {
            UpdateLaserIntensity();
            lastUpdateTime = Time.time;
        }
    }

    public override void ResetAttributes()
    {
        base.ResetAttributes();
        totalIntensity = 0;
        lastUpdateTime = 0f;
    }

    // 重写 OnLaserHit 方法来处理输入激光
    public override List<Laser> OnLaserHit(Laser laser)
    {
        // 判断激光方向是否来自 blockedDirection，若是则拒绝激光
        if (Vector3.Dot(laser.direction, blockedDirection.normalized) > 0.99f)
        {
            Debug.Log("Blocked laser from direction: " + laser.direction);
            return receivedLasers;
        }

        if (receivedLasers.Count < maxLasers)
        {
            if (!receivedLasers.Contains(laser))
            {
                receivedLasers.Add(laser);
                Debug.Log("Received laser from direction: " + laser.direction);
            }
        }

        return receivedLasers;
    }

    public override List<Laser> OnLaserOut(Laser laser)
    {
        if (receivedLasers.Contains(laser))
        {
            receivedLasers.Remove(laser);
            return receivedLasers;
        }

        return null;
    }

    // 更新激光强度并发射合并后的激光
    public void UpdateLaserIntensity()
    {
        totalIntensity = 0;

        // 计算接收到的所有激光的总强度
        foreach (var receivedLaser in receivedLasers)
        {
            totalIntensity += receivedLaser.intensity;
        }

        // 限制总强度不能超过最大值
        if (totalIntensity > maxTotalIntensity)
        {
            totalIntensity = maxTotalIntensity;
        }

        // 如果有接收到激光，发射合并后的激光
        if (receivedLasers.Count > 0)
        {
            // 检查该塔是否已经有输出激光，如果没有则创建一个新的
            List<Laser> emittedLasers = laserManager.GetLaserForTower(this);
            if (emittedLasers == null || emittedLasers.Count == 0)
            {
                laserManager.CreateLaser(this, transform.position, outputDirection, totalIntensity);
            }
            else
            {
                // 如果已经有输出激光，则更新其强度
                foreach (var laser in emittedLasers)
                {
                    laser.SetLaserProperties(totalIntensity, outputDirection);
                }
            }
        }
    }
}

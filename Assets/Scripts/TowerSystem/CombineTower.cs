using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineTower : Tower
{
    public int maxLasers = 7; // 最大可接收激光数量
    private float totalIntensity = 0f; // 累计的激光强度
    private float maxTotalIntensity = 200f; // 最大激光强度限制
    private float updateInterval = 0.1f; // 更新激光强度的间隔
    private float lastUpdateTime = 0f;

    private LaserManager laserManager;
    private Laser emittedLaser; // 发射出的激光
    private Vector3 emittedDirection = Vector3.down; // 固定向下方向

    public override void Initialize()
    {
        base.Initialize();
        laserManager = FindObjectOfType<LaserManager>();
    }

    public override void UpdateState()
    {
        // 每隔一定时间更新合并后的激光强度
        if (Time.time - lastUpdateTime >= updateInterval)
        {
            UpdateLaserIntensity();
            lastUpdateTime = Time.time;
        }
    }

    public override void ResetAttributes()
    {
        base.ResetAttributes();
        totalIntensity = 0f;
        lastUpdateTime = 0f;
        receivedLasers.Clear(); // 清空接收到的激光列表

        // 如果之前有发射的激光，移除它
        if (emittedLaser != null)
        {
            laserManager.RemoveLaser(this);
            emittedLaser = null;
        }
    }

    // 重写 OnLaserHit 方法，处理激光击中合光器
    public override void OnLaserHit(Laser laser)
    {
        if (laser.sourceTower == this || receivedLasers.Contains(laser))
            return;

        // 检查是否与发射方向平行（忽略平行的激光）
        if (Vector3.Dot(laser.direction, emittedDirection) > 0.9f) // 0.9 表示近似平行
        {
            return;
        }

        // 如果还未达到最大接收激光数量，则接收新的激光
        if (receivedLasers.Count < maxLasers)
        {
            receivedLasers.Add(laser);
        }

        // 当收到第一条激光时，创建向下发射的激光
        if (receivedLasers.Count == 1 && emittedLaser == null)
        {
            emittedLaser = laserManager.CreateLaser(this, transform.position, emittedDirection, 0); // 初始强度设为 0
        }
    }

    public override void OnLaserOut(Laser laser)
    {
        if (receivedLasers.Contains(laser))
        {
            receivedLasers.Remove(laser);
        }

        // 如果所有接收的激光都消失，则移除发射激光
        if (receivedLasers.Count == 0 && emittedLaser != null)
        {
            laserManager.RemoveLaser(this);
            emittedLaser = null;
        }
    }

    // 更新合并后发出的激光强度
    public void UpdateLaserIntensity()
    {
        // 累加接收到的激光强度
        totalIntensity = 0f; // 重置总强度
        foreach (var receivedLaser in receivedLasers)
        {
            totalIntensity += receivedLaser.intensity;
        }

        // 限制合并后的总强度不能超过最大值
        if (totalIntensity > maxTotalIntensity)
        {
            totalIntensity = maxTotalIntensity;
        }

        // 更新发射激光的强度
        if (emittedLaser != null)
        {
            emittedLaser.SetLaserProperties(totalIntensity, emittedDirection);
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        // 检查碰撞的对象是否是 Laser，并且是否带有 "Laser" 标签
        if (collision.CompareTag("Laser"))
        {
            Laser laser = collision.GetComponent<Laser>();
            if (laser != null)
            {
                // 调用塔的 OnLaserHit 方法处理激光击中
                OnLaserHit(laser);
            }
        }
    }
    
    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Laser"))
        {
            Laser laser = collision.GetComponent<Laser>();
            if (laser != null)
            {
                // 调用塔的 OnLaserHit 方法处理激光击中
                OnLaserOut(laser);
            }
        }
    }
}

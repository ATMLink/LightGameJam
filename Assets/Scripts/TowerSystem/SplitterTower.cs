using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitterTower : Tower
{
    public int numberOfLasers = 2;
    // public float decay = 0.5f;
    private float totalIntensity = 0;
    private float maxTotalIntensity = 100f;
    private float updateInterval = 0.1f; // 每隔0.1秒更新一次激光强度
    private float lastUpdateTime = 0f;

    private LaserManager laserManager;

    public override void Initialize()
    {
        base.Initialize();
        laserManager = FindObjectOfType<LaserManager>();
    }

    public override void UpdateState()
    {
        // 计算并更新强度
        if (Time.time - lastUpdateTime >= updateInterval)
        {
            UpdateLaserIntensity();
            lastUpdateTime = Time.time; // 记录上一次更新的时间
        }
    }

    public override void ResetAttributes()
    {
        base.ResetAttributes();
        totalIntensity = 0;
        lastUpdateTime = 0f;
    }

    // 重写 OnLaserHit 方法
    public override List<Laser> OnLaserHit(Laser laser)
    {
        if (receivedLasers.Count <= 0)
        {
            receivedLasers.Add(laser);
        }

        if (receivedLasers.Count == 1)
        {
            Vector3 originalDirection = laser.direction;

            Vector3 referenceVector = Vector3.up;

            // 与原始激光垂直的两个方向
            Vector3 outLaserDirection1 = Vector3.Cross(originalDirection, referenceVector).normalized;
            Vector3 outLaserDirection2 = Vector3.Cross(originalDirection, outLaserDirection1).normalized;
            
            // 创建激光
            laserManager.CreateLaser(this, transform.position,
                outLaserDirection1, laser.intensity / 2);
            laserManager.CreateLaser(this, transform.position,
                outLaserDirection2, laser.intensity / 2);
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
    

    public void UpdateLaserIntensity()
    {
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
        
        // 计算分光器发出的激光强度，并更新激光属性
        float emittedIntensity = totalIntensity / numberOfLasers;

        // 获取与该塔关联的激光并更新其强度
        List<Laser> emittedLasers = laserManager.GetLaserForTower(this);
        if (emittedLasers != null)
        {
            foreach (var laser in emittedLasers)
            {
                laser.SetLaserProperties(emittedIntensity, laser.direction);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
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
    
    private void OnTriggerExit2D(Collider2D collision)
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

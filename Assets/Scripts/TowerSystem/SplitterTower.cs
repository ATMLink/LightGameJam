using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitterTower : Tower
{
    // public int numberOfLasers = 2;
    // // public float decay = 0.5f;
    // private float totalIntensity = 0;
    // private float maxTotalIntensity = 100f;
    // private float updateInterval = 0.1f; // 每隔0.1秒更新一次激光强度
    // private float lastUpdateTime = 0f;
    //
    // private LaserManager laserManager;
    //
    // public override void Initialize()
    // {
    //     base.Initialize();
    //     laserManager = FindObjectOfType<LaserManager>();
    // }
    //
    // public override void UpdateState()
    // {
    //     // 计算并更新强度
    //     if (Time.time - lastUpdateTime >= updateInterval)
    //     {
    //         UpdateLaserIntensity();
    //         lastUpdateTime = Time.time; // 记录上一次更新的时间
    //     }
    // }
    //
    // public override void ResetAttributes()
    // {
    //     base.ResetAttributes();
    //     totalIntensity = 0;
    //     lastUpdateTime = 0f;
    // }
    //
    // // 重写 OnLaserHit 方法
    // public override void OnLaserHit(Laser laser)
    // {
    //     if (laser.sourceTower == this)
    //         return;
    //     
    //     if (receivedLasers.Count <= 0)
    //     {
    //         receivedLasers.Add(laser);
    //     }
    //     
    //     if (receivedLasers.Count == 1)
    //     {
    //         Vector3 originalDirection = laser.direction;
    //
    //         Vector3 referenceVector = Vector3.up;
    //
    //         // 与原始激光垂直的两个方向
    //         Vector3 outLaserDirection1 = Vector3.Cross(originalDirection, referenceVector).normalized;
    //         Vector3 outLaserDirection2 = Vector3.Cross(originalDirection, outLaserDirection1).normalized;
    //         
    //         // 创建激光
    //         laserManager.CreateLaser(this, transform.position,
    //             outLaserDirection1, laser.intensity / 2);
    //         laserManager.CreateLaser(this, transform.position,
    //             outLaserDirection2, laser.intensity / 2);
    //     }
    // }
    //
    // public override void OnLaserOut(Laser laser)
    // {
    //     if (receivedLasers.Contains(laser))
    //     {
    //         receivedLasers.Remove(laser);
    //     }
    // }
    //
    //
    // public void UpdateLaserIntensity()
    // {
    //     // 计算接收到的所有激光的总强度
    //     foreach (var receivedLaser in receivedLasers)
    //     {
    //         totalIntensity += receivedLaser.intensity;
    //     }
    //
    //     // 限制总强度不能超过最大值
    //     if (totalIntensity > maxTotalIntensity)
    //     {
    //         totalIntensity = maxTotalIntensity;
    //     }
    //     
    //     // 计算分光器发出的激光强度，并更新激光属性
    //     float emittedIntensity = totalIntensity / numberOfLasers;
    //
    //     // 获取与该塔关联的激光并更新其强度
    //     List<Laser> emittedLasers = laserManager.GetLaserForTower(this);
    //     if (emittedLasers != null)
    //     {
    //         foreach (var laser in emittedLasers)
    //         {
    //             laser.SetLaserProperties(emittedIntensity, laser.direction);
    //         }
    //     }
    // }
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
    Debug.Log("SplitterTower initialized.");
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
    Debug.Log("SplitterTower attributes reset.");
}

public override void OnLaserHit(Laser laser)
{
    if (laser.sourceTower == this)
    {
        Debug.Log("激光来自本塔，不执行OnLaserHit处理。");
        return;
    }

    if (receivedLasers != null)
    {
        receivedLasers.Add(laser);
        Debug.Log($"接收到激光，方向: {laser.direction}, 强度: {laser.intensity}, 激光数量: {receivedLasers.Count}");
    }

    if (receivedLasers.Count == 1)
    {
        Vector3 originalDirection = laser.direction;
        Vector3 referenceVector = Vector3.up;

        Vector3 outLaserDirection1 = Vector3.Cross(originalDirection, referenceVector).normalized;
        Vector3 outLaserDirection2 = Vector3.Cross(originalDirection, outLaserDirection1).normalized;

        Debug.Log($"分光器发射两束激光，方向1: {outLaserDirection1}, 方向2: {outLaserDirection2}");

        laserManager.CreateLaser(this, transform.position, outLaserDirection1, laser.intensity / 2);
        laserManager.CreateLaser(this, transform.position, outLaserDirection2, laser.intensity / 2);
    }
}

public override void OnLaserOut(Laser laser)
{
    if (receivedLasers.Contains(laser))
    {
        receivedLasers.Remove(laser);
        Debug.Log("激光离开分光器，剩余接收激光数量: " + receivedLasers.Count);
    }
}

public void UpdateLaserIntensity()
{
    totalIntensity = 0;

    foreach (var receivedLaser in receivedLasers)
    {
        totalIntensity += receivedLaser.intensity;
    }

    if (totalIntensity > maxTotalIntensity)
    {
        totalIntensity = maxTotalIntensity;
    }

    Debug.Log($"分光器总接收强度: {totalIntensity}");

    float emittedIntensity = totalIntensity / numberOfLasers;
    Debug.Log($"分光器发出的激光强度: {emittedIntensity}");

    List<Laser> emittedLasers = laserManager.GetLaserForTower(this);
    if (emittedLasers != null)
    {
        foreach (var laser in emittedLasers)
        {
            laser.SetLaserProperties(emittedIntensity, laser.direction);
            Debug.Log($"更新发射激光的强度: {emittedIntensity}, 方向: {laser.direction}");
        }
    }
}

protected void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Laser"))
    {
        Laser laser = collision.GetComponent<Laser>();
        if (laser != null)
        {
            Debug.Log($"检测到碰撞对象为激光，方向: {laser.direction}, 强度: {laser.intensity}");
            OnLaserHit(laser);
        }
        else
        {
            Debug.Log("碰撞对象无激光组件。");
        }
    }
    else
    {
        Debug.Log("碰撞对象非激光。");
    }
}

protected void OnTriggerExit2D(Collider2D collision)
{
    if (collision.CompareTag("Laser"))
    {
        Laser laser = collision.GetComponent<Laser>();
        if (laser != null)
        {
            Debug.Log($"激光离开，方向: {laser.direction}, 强度: {laser.intensity}");
            OnLaserOut(laser);
        }
        else
        {
            Debug.Log("离开碰撞区域的对象无激光组件。");
        }
    }
}
}

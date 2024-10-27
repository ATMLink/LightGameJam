using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public class SplitterTower : Tower
// {
//     
//     public int numberOfLasers = 2;
// // public float decay = 0.5f;
// private float totalIntensity = 0;
// private float maxTotalIntensity = 100f;
// private float updateInterval = 0.1f; // 每隔0.1秒更新一次激光强度
// private float lastUpdateTime = 0f;
//
// // private LaserManager laserManager;
//
// public override void Initialize()
// {
//     base.Initialize();
//     laserManager = FindObjectOfType<LaserManager>();
//     Debug.Log("SplitterTower initialized.");
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
//     Debug.Log("SplitterTower attributes reset.");
// }
//
// public override void OnLaserHit(Laser laser)
// {
//     // if (laser.sourceTower == this)
//     // {
//     //     Debug.Log("激光来自本塔，不执行OnLaserHit处理。");
//     //     return;
//     // }
//     if (!receivedLasers.Contains(laser))
//     {
//         receivedLasers.Add(laser);
//     }
//
//     if (receivedLasers.Count == 1||(laserManager.GetLaserForTower(this) == null && receivedLasers.Count == 2)) // 只在接收到一条激光时生成分光
//     {
//         Vector3 originalDirection = laser.direction.normalized;
//
//         // 计算与入射光线垂直的两个方向，方向相反
//         Vector3 outLaserDirection1 = Vector3.Cross(originalDirection, Vector3.forward).normalized;
//         Vector3 outLaserDirection2 = -outLaserDirection1;
//
//         Vector3 laserOriginOffset = outLaserDirection1.normalized * 0.51f;
//         Debug.Log($"分光器发射两束激光，方向1: {outLaserDirection1}, 方向2: {outLaserDirection2}");
//
//         // 使用半强度创建两条新激光
//         laserManager.CreateLaser(this, transform.position + laserOriginOffset, outLaserDirection1, laser.intensity / 2);
//         laserOriginOffset = outLaserDirection2 * 0.51f;
//         laserManager.CreateLaser(this, transform.position + laserOriginOffset, outLaserDirection2, laser.intensity / 2);
//     }
// }
//
// public override void OnLaserOut(Laser laser)
// {
//     if (receivedLasers.Contains(laser))
//     {
//         laser.UpdateState();
//         receivedLasers.Remove(laser);
//         Debug.Log("激光离开分光器，剩余接收激光数量: " + receivedLasers.Count);
//     }
//
//     if (receivedLasers.Count == 0)
//     {
//         laserManager.RemoveLaser(this);
//     }
// }
//
// public void UpdateLaserIntensity()
// {
//     totalIntensity = 0;
//     foreach (var receivedLaser in receivedLasers)
//     {
//         totalIntensity += receivedLaser.intensity;
//     }
//
//     if (totalIntensity > maxTotalIntensity)
//     {
//         totalIntensity = maxTotalIntensity;
//     }
//
//     Debug.Log($"分光器总接收强度: {totalIntensity} (接收的激光数量: {receivedLasers.Count})");
//
//     float emittedIntensity = totalIntensity / numberOfLasers;
//     Debug.Log($"分光器发出的激光强度: {emittedIntensity}");
//
//     List<Laser> emittedLasers = laserManager.GetLaserForTower(this);
//     if (emittedLasers != null)
//     {
//         foreach (var laser in emittedLasers)
//         {
//             laser.SetLaserProperties(emittedIntensity, laser.direction);
//             Debug.Log($"更新发射激光的强度: {emittedIntensity}, 方向: {laser.direction}");
//         }
//     }
// }

//protected void OnTriggerEnter2D(Collider2D collision)
//{
//    if (collision.CompareTag("Laser"))
//    {
//        Laser laser = collision.GetComponent<Laser>();
//        if (laser != null)
//        {
//            Debug.Log($"检测到碰撞对象为激光，方向: {laser.direction}, 强度: {laser.intensity}");
//            OnLaserHit(laser);
//        }
//        else
//        {
//            Debug.Log("碰撞对象无激光组件。");
//        }
//    }
//    else
//    {
//        Debug.Log("碰撞对象非激光。");
//    }
//}

//protected void OnTriggerExit2D(Collider2D collision)
//{
//    if (collision.CompareTag("Laser"))
//    {
//        Laser laser = collision.GetComponent<Laser>();
//        if (laser != null)
//        {
//            Debug.Log($"激光离开，方向: {laser.direction}, 强度: {laser.intensity}");
//            OnLaserOut(laser);
//        }
//        else
//        {
//            Debug.Log("离开碰撞区域的对象无激光组件。");
//        }
//    }
//}
// }
public class SplitterTower : Tower
{
    public int numberOfLasers = 2;
    private float totalIntensity = 0;
    private float maxTotalIntensity = 5000f;
    private float updateInterval = 0.1f;
    private float lastUpdateTime = 0f;

    private bool onUse = false;

    public override void Initialize()
    {
        base.Initialize();
        laserManager = FindObjectOfType<LaserManager>();
        Debug.Log("SplitterTower initialized.");
    }

    public override void UpdateState()
    {
        if (receivedLasers.Count > 0 && Time.time - lastUpdateTime >= updateInterval)
        {
            Debug.Log($"received laser count {receivedLasers.Count}");
            UpdateLaserIntensity();
            ReceivedLaserIntensityIsZero();
            lastUpdateTime = Time.time;
        }
    }

    public override void OnLaserHit(Laser laser)
    {
        if (!receivedLasers.Contains(laser))
        {
            if (laser.sourceTower == this)
                return;            
            receivedLasers.Add(laser);
            Debug.Log("激光接收: " + laser.gameObject.name);
        }

        if (receivedLasers.Count == 1 || (laserManager.GetLaserForTower(this) == null && receivedLasers.Count == 2))
        {
            EmitLasers(laser);
        }
    }

    public override void OnLaserOut(Laser laser)
    {
        //Debug.Log("!!!!!!");
        //Debug.Log("laser out");
        //Debug.Log("!!!!!!!!");
        if (onUse == false) return;
        if (receivedLasers.Contains(laser))
        {
            receivedLasers.Remove(laser);
            Debug.Log("激光离开: " + laser.gameObject.name);
        }

        if (receivedLasers.Count == 0)
        {
            onUse = false;
            laserManager.RemoveLaser(this);
            Debug.Log("没有接收到激光，停止发射。");
        }
    }

    private void EmitLasers(Laser laser)
    {
        Vector3 originalDirection = laser.direction.normalized;
        Vector3 outLaserDirection1 = Vector3.Cross(originalDirection, Vector3.forward).normalized;
        Vector3 outLaserDirection2 = -outLaserDirection1;

        Vector3 laserOriginOffset1 = outLaserDirection1 * 0.51f;
        laserManager.CreateLaser(this, transform.position + laserOriginOffset1, outLaserDirection1, laser.intensity / 2);
        Vector3 laserOriginOffset2 = outLaserDirection2 * 0.51f;
        laserManager.CreateLaser(this, transform.position + laserOriginOffset2, outLaserDirection2, laser.intensity / 2);
        Debug.Log("分光器发射两束激光");
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

        float emittedIntensity = totalIntensity / numberOfLasers;
        List<Laser> emittedLasers = laserManager.GetLaserForTower(this);
        if (emittedLasers != null)
        {
            foreach (var laser in emittedLasers)
            {
                laser.SetLaserIntensity(emittedIntensity);
                Debug.Log($"更新发射激光的强度: {emittedIntensity}, 方向: {laser.direction}");
            }
        }
        else
        {
            Debug.Log("没有发射激光，无法更新强度。");
        }
    }
    private void ReceivedLaserIntensityIsZero()
    {
        if (totalIntensity > 0.1f)
            return;
        laserManager.RemoveLaser(this);
    }
}

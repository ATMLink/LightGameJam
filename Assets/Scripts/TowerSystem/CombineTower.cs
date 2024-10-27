using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public class CombineTower : Tower
// {
//     public int maxLasers = 7; // 最大接收激光数
//     private float totalIntensity = 0f; // 合并激光强度
//     private float maxTotalIntensity = 200f; // 最大合并强度
//     private float updateInterval = 0.1f; // 更新激光强度的间隔
//     private float lastUpdateTime = 0f;
//
//     // private LaserManager laserManager;
//     private Laser emittedLaser; // 由此塔发射的激光
//     private Vector3 emittedDirection = Vector3.down; // 固定向下的发射方向
//
//     public override void Initialize()
//     {
//         base.Initialize();
//         laserManager = FindObjectOfType<LaserManager>();
//         Debug.Log("CombineTower 初始化完成。");
//     }
//
//     public override void UpdateState()
//     {
//         if (Time.time - lastUpdateTime >= updateInterval)
//         {
//             UpdateLaserIntensity();
//             lastUpdateTime = Time.time;
//         }
//     }
//
//     public override void ResetAttributes()
//     {
//         base.ResetAttributes();
//         totalIntensity = 0f;
//         lastUpdateTime = 0f;
//         receivedLasers.Clear();
//         Debug.Log("CombineTower 属性重置完成。");
//
//         if (emittedLaser != null)
//         {
//             laserManager.RemoveLaser(this);
//             emittedLaser = null;
//             Debug.Log("重置时移除发射激光。");
//         }
//     }
//
//     public override void OnLaserHit(Laser laser)
//     {
//         if (laser.sourceTower == this || receivedLasers.Contains(laser))
//         {
//             Debug.Log("忽略激光：自发或已添加。");
//             return;
//         }
//
//         if (receivedLasers.Count <= maxLasers)
//         {
//             if (laser.sourceTower == this)
//                 return;
//             receivedLasers.Add(laser);
//             Debug.Log($"激光添加至 CombineTower。当前总激光数：{receivedLasers.Count}");
//         }
//         
//         if (receivedLasers.Count == 1 && emittedLaser == null)
//         {
//             Vector3 laserOriginOffset = emittedDirection.normalized * 0.51f;
//
//             // 使用半强度创建两条新激光
//             laserManager.CreateLaser(this, transform.position + laserOriginOffset, emittedDirection, laser.intensity);
//         }
//     }
//
//     public override void OnLaserOut(Laser laser)
//     {
//         if (receivedLasers.Contains(laser))
//         {
//             receivedLasers.Remove(laser);
//             Debug.Log("激光从 CombineTower 移除。");
//         }
//
//         if (receivedLasers.Count == 0)
//         {
//             laserManager.RemoveLaser(this);
//             emittedLaser = null;
//             Debug.Log("没有收到激光，移除发射激光。");
//         }
//     }
//
//     public void UpdateLaserIntensity()
//     {
//         totalIntensity = 0f;
//         foreach (var receivedLaser in receivedLasers)
//         {
//             totalIntensity += receivedLaser.intensity;
//             Debug.Log($"接收的激光强度：{receivedLaser.intensity}，当前总强度：{totalIntensity}");
//         }
//
//         totalIntensity = Mathf.Min(totalIntensity, maxTotalIntensity);
//         Debug.Log($"总接收强度封顶后：{totalIntensity}");
//
//         if (emittedLaser != null)
//         {
//             emittedLaser.SetLaserProperties(totalIntensity, emittedDirection);
//             Debug.Log($"更新发射激光强度：{totalIntensity}，方向：{emittedDirection}");
//         }
//         else
//         {
//             Debug.LogWarning("未找到发射激光来更新强度。");
//         }
//     }

    //protected void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Laser"))
    //    {
    //        Laser laser = collision.GetComponent<Laser>();
    //        if (laser != null)
    //        {
    //            Debug.Log($"激光进入 CombineTower 的触发器。方向：{laser.direction}，强度：{laser.intensity}");
    //            OnLaserHit(laser);
    //        }
    //        else
    //        {
    //            Debug.LogWarning("标记为激光的碰撞对象没有激光组件。");
    //        }
    //    }
    //}
    
    //protected void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Laser"))
    //    {
    //        Laser laser = collision.GetComponent<Laser>();
    //        if (laser != null)
    //        {
    //            Debug.Log($"激光退出 CombineTower 的触发器。方向：{laser.direction}，强度：{laser.intensity}");
    //            OnLaserOut(laser);
    //        }
    //        else
    //        {
    //            Debug.LogWarning("退出时标记为激光的碰撞对象没有激光组件。");
    //        }
    //    }
    //}
// }
public class CombineTower : Tower
{
    public int maxLasers = 7; // 最大接收激光数
    private float totalIntensity = 0f; // 合并激光强度
    private float maxTotalIntensity = 5000f; // 最大合并强度
    private float updateInterval = 0.1f; // 更新激光强度的间隔
    private float lastUpdateTime = 0f;

    private Laser emittedLaser; // 由此塔发射的激光
    private Vector3 emittedDirection = Vector3.down; // 固定向下的发射方向

    public override void Initialize()
    {
        base.Initialize();
        laserManager = FindObjectOfType<LaserManager>();
        Debug.Log("CombineTower 初始化完成。");
    }

    public override void UpdateState()
    {
        if (Time.time - lastUpdateTime >= updateInterval && receivedLasers.Count > 0)
        {
            UpdateLaserIntensity();
            ReceivedLaserIntensityIsZero();
            lastUpdateTime = Time.time;
            
        }
        Debug.Log(transform.rotation);
        Debug.Log(Vector3.down);
    }

    public override void ResetAttributes()
    {
        base.ResetAttributes();
        totalIntensity = 0f;
        lastUpdateTime = 0f;
        receivedLasers.Clear();
        Debug.Log("CombineTower 属性重置完成。");

        if (emittedLaser != null)
        {
            laserManager.RemoveLaser(this);
            emittedLaser = null;
            Debug.Log("重置时移除发射激光。");
        }
    }

    public override void OnLaserHit(Laser laser)
    {
        if (laser.sourceTower == this || receivedLasers.Contains(laser))
        {
            Debug.Log("忽略激光：自发或已添加。");
            return;
        }

        if (receivedLasers.Count < maxLasers)
        {
            receivedLasers.Add(laser);
            Debug.Log($"激光添加至 CombineTower。当前总激光数：{receivedLasers.Count}");

            // 发射激光逻辑
            if (receivedLasers.Count == 1 && emittedLaser == null)
            {
                Vector3 laserOriginOffset = emittedDirection.normalized * 0.51f;
                emittedLaser = laserManager.CreateLaser(this, transform.position + laserOriginOffset, emittedDirection, laser.intensity);
                Debug.Log("发射新的激光。");
            }
        }
    }

    public override void OnLaserOut(Laser laser)
    {
        if (laser.sourceTower == this) return;
        if (receivedLasers.Contains(laser))
        {
            receivedLasers.Remove(laser);
            Debug.Log("激光从 CombineTower 移除。");
        }

        if (receivedLasers.Count == 0)
        {
            laserManager.RemoveLaser(this);
            emittedLaser = null; // 确保移除发射激光
            Debug.Log("没有收到激光，移除发射激光。");
        }
    }

    public void UpdateLaserIntensity()
    {
        totalIntensity = 0f;
        foreach (var receivedLaser in receivedLasers)
        {
            totalIntensity += receivedLaser.intensity;
            Debug.Log($"接收的激光强度：{receivedLaser.intensity}，当前总强度：{totalIntensity}");
        }

        totalIntensity = Mathf.Min(totalIntensity, maxTotalIntensity);
        Debug.Log($"总接收强度封顶后：{totalIntensity}");

        if (emittedLaser != null)
        {
            emittedLaser.SetLaserIntensity(totalIntensity);
            Debug.Log($"更新发射激光强度：{totalIntensity}，方向：{emittedDirection}");
        }
        else
        {
            Debug.LogWarning("未找到发射激光来更新强度。");
        }
    }

    private void ReceivedLaserIntensityIsZero()
    {
        if (totalIntensity > 0.1f)
            return;
        laserManager.RemoveLaser(this);
        emittedLaser = null; // 确保移除发射激光
    }
}

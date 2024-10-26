using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitterTower : Tower
{
    public int numberOfLasers = 2;
    private float totalIntensity = 0;
    private float maxTotalIntensity = 100f;
    private float updateInterval = 0.1f;
    private float lastUpdateTime = 0f;

    public override void Initialize()
    {
        base.Initialize();
        laserManager = FindObjectOfType<LaserManager>();
        Debug.Log("SplitterTower initialized.");
    }

    public override void UpdateState()
    {
        // 计算并更新激光强度
        if (Time.time - lastUpdateTime >= updateInterval)
        {
            UpdateLaserIntensity();
            lastUpdateTime = Time.time;
        }
    }

    public override void OnLaserHit(Laser laser)
    {
        if (!receivedLasers.Contains(laser))
        {
            receivedLasers.Add(laser);
            Debug.Log($"Added laser to receivedLasers. Current count: {receivedLasers.Count}");
        }

        // Debug current received lasers
        foreach (var l in receivedLasers)
        {
            Debug.Log($"Received laser direction: {l.direction}, intensity: {l.intensity}");
        }

        // 分光条件
        if (receivedLasers.Count == 1 || (laserManager.GetLaserForTower(this) == null && receivedLasers.Count == 2))
        {
            Vector3 originalDirection = laser.direction.normalized;
            Vector3 outLaserDirection1 = Vector3.Cross(originalDirection, Vector3.forward).normalized;
            Vector3 outLaserDirection2 = -outLaserDirection1;

            Vector3 laserOriginOffset = outLaserDirection1 * 0.51f;
            Debug.Log($"Splitter emitting two beams. Directions: {outLaserDirection1}, {outLaserDirection2}");

            laserManager.CreateLaser(this, transform.position + laserOriginOffset, outLaserDirection1, laser.intensity / 2);
            laserOriginOffset = outLaserDirection2 * 0.51f;
            laserManager.CreateLaser(this, transform.position + laserOriginOffset, outLaserDirection2, laser.intensity / 2);
        }
    }

    public override void OnLaserOut(Laser laser)
    {
        if (receivedLasers.Contains(laser))
        {
            receivedLasers.Remove(laser);
            Debug.Log($"Laser removed from receivedLasers. Current count: {receivedLasers.Count}");
        }

        // 确保当没有接收到激光时停止发射
        if (receivedLasers.Count == 0)
        {
            laserManager.RemoveLaser(this);
            Debug.Log("No lasers received, stopping all emitted lasers.");

            // 确保删除所有已发射的激光
            List<Laser> emittedLasers = laserManager.GetLaserForTower(this);
            if (emittedLasers != null)
            {
                foreach (var emittedLaser in emittedLasers)
                {
                    emittedLaser.SetLaserActive(false);
                    emittedLaser.gameObject.SetActive(false);// 假设你有一个方法可以禁用激光
                    laserManager.RemoveLaser(this); // 返回到激光池
                    Debug.Log("Emitted laser removed.");
                }
            }
        }
    }

    public void UpdateLaserIntensity()
    {
        totalIntensity = 0;

        Debug.Log($"Received lasers count: {receivedLasers.Count}");
        if (receivedLasers.Count == 0)
        {
            laserManager.RemoveLaser(this);
            Debug.Log("No received lasers; all emitted lasers removed.");
            return;
        }

        // 计算总强度
        foreach (var receivedLaser in receivedLasers)
        {
            totalIntensity += receivedLaser.intensity;
        }

        // 限制总强度
        if (totalIntensity > maxTotalIntensity)
        {
            totalIntensity = maxTotalIntensity;
        }

        Debug.Log($"Total received intensity: {totalIntensity}, received laser count: {receivedLasers.Count}");

        float emittedIntensity = totalIntensity / numberOfLasers;
        Debug.Log($"Intensity of emitted lasers: {emittedIntensity}");

        // 更新当前已发射的激光
        List<Laser> emittedLasers = laserManager.GetLaserForTower(this);
        if (emittedLasers != null)
        {
            foreach (var laser in emittedLasers)
            {
                laser.SetLaserProperties(emittedIntensity, laser.direction);
                Debug.Log($"Updated emitted laser intensity: {emittedIntensity}, direction: {laser.direction}");
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
                Debug.Log($"Laser entered collider. Direction: {laser.direction}, Intensity: {laser.intensity}");
                OnLaserHit(laser);
            }
            else
            {
                Debug.Log("Collider object has no Laser component.");
            }
        }
        else
        {
            Debug.Log("Collider object is not a laser.");
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Laser"))
        {
            Laser laser = collision.GetComponent<Laser>();
            if (laser != null)
            {
                Debug.Log($"Laser exited collider. Direction: {laser.direction}, Intensity: {laser.intensity}");
                OnLaserOut(laser);
            }
            else
            {
                Debug.Log("Exited collider object has no Laser component.");
            }
        }
    }
}

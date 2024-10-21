using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPool : MonoBehaviour
{
    public GameObject laserPrefab; // 激光预制体
    public int initialPoolSize = 8; // 初始激光池的大小
    private List<Laser> laserPool; // 激光池

    public void Initialize()
    {
        laserPool = new List<Laser>();

        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject laserObject = Instantiate(laserPrefab);
            Laser laser = laserObject.GetComponent<Laser>();
            laserPool.Add(laser);
            laser.SetLaserActive(false); // 初始化时隐藏激光
        }
    }

    public Laser GetLaser(Vector3 position, Vector3 direction, float intensity)
    {
        foreach (Laser laser in laserPool)
        {
            if (!laser.gameObject.activeInHierarchy) // 如果激光未激活，则重用
            {
                laser.transform.position = position;
                laser.SetLaserProperties(intensity, direction);
                laser.SetLaserActive(true); // 激活激光
                return laser;
            }
        }

        // 如果没有可用的激光，扩容池
        ExpandPool(1);
        return GetLaser(position, direction, intensity); // 递归调用以获取新的激光
    }

    private void ExpandPool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject laserObject = Instantiate(laserPrefab);
            Laser laser = laserObject.GetComponent<Laser>();
            laser.SetLaserActive(false); // 初始化时隐藏激光
            laserPool.Add(laser);
        }

        Debug.Log($"Laser pool expanded. New size: {laserPool.Count}");
    }

    public void ReturnLaser(Laser laser)
    {
        laser.ResetLaser();
        laser.SetLaserActive(false); // 隐藏激光
    }
}

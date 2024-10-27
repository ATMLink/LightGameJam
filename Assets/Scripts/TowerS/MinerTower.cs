using DG.Tweening.Core.Easing;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerTower : Tower
    {
    //private MainResourceManagement resourceManagement;
    float minIntensity;
    float medaltime = 0;
    float sitime = 0;
    private float totalIntensity = 0;
    // private LaserManager laserManager;
    public override void Initialize()
        {
        base.Initialize();
        laserManager = FindObjectOfType<LaserManager>();
        resourceManagement = GameObject.Find("ResourceManager").GetComponent<MainResourceManagement>();
        }
    public override void UpdateState()
        {
        base.UpdateState();
        UpdateLaserIntensity();
        Generate();
        if (attributes.name == "Miner") minIntensity = 0;
        else if (attributes.name == "LaserTower") minIntensity = 0;
        }
    public override void OnLaserHit(Laser laser)
        {
        if (laser.sourceTower == this)
            return;
        receivedLasers.Add(laser);
        }
    public void UpdateLaserIntensity()
        {
        float temp = 0;
        // 计算接收到的所有激光的总强度
        foreach (var receivedLaser in receivedLasers)
            {
            temp += receivedLaser.intensity;
            }
        totalIntensity = temp;
        }
    public override void Upgrade()
        {
        base.Upgrade();
        Tower tower = GetComponent<Tower>();
        if (attributes.name == "LaserTower")
            {
            laserManager.CreateLaser(tower, transform.position, Vector3.down, 300);
            }

        }
    public void Generate()
        {

        if (totalIntensity >= minIntensity)
            {
            //Debug.LogWarning("work");
            sitime += Time.deltaTime;
            medaltime += Time.deltaTime;
            if (sitime >= 5)
                {
                resourceManagement.CollectResource(element.si, 20);
                sitime = 0;
                }
            if (medaltime >= 30)

                for (int i = 0; i < 5; i++)
                    {
                    int a = Random.Range(0, 5);
                    switch (a)
                        {
                        case 0: resourceManagement.CollectResource(element.na, 1); break;
                        case 1: resourceManagement.CollectResource(element.k, 1); break;
                        case 2: resourceManagement.CollectResource(element.li, 1); break;
                        case 3: resourceManagement.CollectResource(element.cu, 1); break;
                        case 4: resourceManagement.CollectResource(element.cs, 1); break;
                        }
                    medaltime = 0;
                    }
            }


        }



    }


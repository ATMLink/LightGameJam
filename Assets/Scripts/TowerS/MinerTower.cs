using DG.Tweening.Core.Easing;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class MinerTower : Tower
    {
    //private MainResourceManagement resourceManagement;
    bool a = true;
    float minIntensity;
    float medaltime = 0;
    float sitime = 0;
    private float totalIntensity = 0;
    public override void ResetAttributes()
    {
        base.ResetAttributes();
        a= true;
    }
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
        if (attributes.name == "LaserTower" && a)
        {
            laserManager.CreateLaser(tower, transform.position, Vector3.down, 100);
            a = false;
        }

        }
    public void Generate()
        {
        if (attributes.name == "Miner")
            {
            if (totalIntensity >= minIntensity)
                {
                //Debug.LogWarning("work");
                sitime += Time.deltaTime;
                medaltime += Time.deltaTime;
                if (sitime >= 5)
                    {
                    resourceManagement.CollectResource(element.si, 5);
                    reflectionManager.Reflect("+5 Si", transform.position, Color.green);
                    sitime = 0;
                    }
                if (medaltime >= 60)
                {
                    StartCoroutine(getResource());
                    medaltime = 0;
                }
                }
            }
        

        }

    private IEnumerator getResource()
    {
        float delay = 1f;
        while (true)
        {
            delay -= Time.deltaTime;
            if (delay <= 0)
            {
                for (int i = 0; i < 1; i++)
                {
                    int a = Random.Range(0, 3);//生成一个稀有资源 
                    switch (a)
                    {
                        case 0: resourceManagement.CollectResource(element.na, 1); reflectionManager.Reflect("+1 Na", transform.position, Color.yellow); break;
                        case 1: resourceManagement.CollectResource(element.k, 1); reflectionManager.Reflect("+1 K", transform.position, new Color(155, 0, 155)); break;
                        case 2: resourceManagement.CollectResource(element.cu, 1); reflectionManager.Reflect("+1 Cu", transform.position, Color.green); break;
                            //case 3: resourceManagement.CollectResource(element.cs, 1); reflectionManager.Reflect("+1 Cs", transform.position, Color.blue); break;
                            //case 4: resourceManagement.CollectResource(element.li, 1); reflectionManager.Reflect("+1 Li", transform.position, Color.red); break;
                    }
                    medaltime = 0;
                }
                break;
            }
            yield return null;
        }

        delay = 1f;

        while (true)
        {
            delay -= Time.deltaTime;
            if (delay <= 0)
            {
                int b = Random.Range(0, 2);//生成一个稀有资源 
                switch (b)
                {
                    case 0: resourceManagement.CollectResource(element.cs, 1); reflectionManager.Reflect("+1 Cs", transform.position, new Color(0, 65, 30)); break;
                    case 1: resourceManagement.CollectResource(element.li, 1); reflectionManager.Reflect("+1 Li", transform.position, Color.red); break;
                }
                break;
            }
            yield return null;
        }
    }


    }


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RouterTower : Tower
{
    [SerializeField]
    private ParticleSystem particle;
    [SerializeField]
    private RouterSight sight2;

    protected bool canShow = false;

    private int towerCount = 0;

    public override void UpdateState()
    {
        if (canShow)
        {
            var emission = particle.emission;
            emission.rateOverTime = 50;
            if (sight2.towerInSight.Count != towerCount)
            {
                foreach (var tower in sight2.towerInSight)
                {
                    Debug.Log("add");
                    tower.AddRouterToTower(this);
                }
                towerCount = sight2.towerInSight.Count;
            }
        }
        else
        {
            var emission = particle.emission;
            emission.rateOverTime = 0;
        }
    }

    public override void Initialize()
    {
        health = attributes.health.Value;

        receivedLasers = new List<Laser>();
        transform.rotation = Quaternion.Euler(Vector3.down);

        resourceManagement = GameObject.Find("ResourceManager").GetComponent<MainResourceManagement>();
        for (int i = 0; i < attributes.elements.Count; i++)
        {
            resourceManagement.SpendResoure(attributes.elements[i], attributes.elementSpendNumber[i]);
        }

        towerID = towerIDCounter++;
        laserManager = FindObjectOfType<LaserManager>();

        sight2.towerInSight.Clear();

        gameObject.SetActive(true);
        var emission = particle.emission;
        emission.rateOverTime = 0;
    }


    public override void OnLaserHit(Laser laser)
    {
        if (receivedLasers != null)
        {
            receivedLasers.Add(laser);
        }
        if (!canShow)
        {
            float inten = 0;
            foreach (var lasr in receivedLasers)
            {
                inten += lasr.intensity;
            }
            if (inten >= 100f) canShow = true;
        }
    }

    public override void OnLaserOut(Laser laser)
    {
        if (receivedLasers.Contains(laser))
        {
            receivedLasers.Remove(laser);
        }
        if (canShow)
        {
            float inten = 0;
            foreach (var lasr in receivedLasers)
            {
                inten += lasr.intensity;
            }
            if (inten < 100f)
            {
                canShow = false;
                foreach (var tower in sight2.towerInSight)
                {
                    tower.RemoveRouterToTower(this);
                }
                towerCount = 0;
            }
        }
    }


    public override void DestroyTower()
    {
        ResetAttributes();
        laserManager.RemoveLaser(this);

        Debug.Log("remove");
        foreach (var tower in sight2.towerInSight)
        {
            tower.RemoveRouterToTower(this);
        }

        gameObject.SetActive(false); // 将塔移回对象池
    }

    public override void RemoveTower()
    {
        foreach (var tower in sight2.towerInSight)
        {
            tower.RemoveRouterToTower(this);
        }
    }

}

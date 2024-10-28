using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AreaLight : Tower
{
    Light2D light2d;//用来改状态的light2d
    protected bool lightOn = false;
    LightSystem ls;
    float inten = 0;

    private float debug_light_outer = 0;
    public override void Initialize()
    {
        base.Initialize();

        light2d = this.gameObject.GetComponent<Light2D>();
        Debug.Log("照明灯获取light2d成功");

        light2d.enabled = false;

        LightSystem.Instance.AddLight(light2d);
        Debug.Log("照明灯初始化成功");
    }


    public override void UpdateState()
    {
        if (inten >= 10f || routerTowerList.Count > 0)
        {
            light2d.pointLightOuterRadius = 4;
            lightOn = true;
            light2d.enabled = true;
            float light_record = Mathf.Sqrt(inten + routerTowerList.Count * 10 - 10) / 2 + 4;
            light2d.pointLightOuterRadius = light_record;

            if (debug_light_outer != light_record)
            {

                debug_light_outer = light_record;
                Debug.Log("照明强度为" + light_record);
            }

        }
        else
        {
            lightOn = false;
            light2d.enabled = false;
        }
    }

    public override void RemoveRouterToTower(RouterTower router)
    {
        if (routerTowerList.Contains(router))
        {
            routerTowerList.Remove(router);
            if (routerTowerList.Count == 0)
            {
                float inten = 0;
                foreach (var lasr in receivedLasers)
                {
                    inten += lasr.intensity;
                }
            }
        }
    }

    public override void OnLaserHit(Laser laser)//激光进入时
    {
        if (receivedLasers != null)
        {
            receivedLasers.Add(laser);//在接收列表中添加激光
        }
        foreach (var lasers in receivedLasers)
        {
            inten = 0;
            inten += lasers.intensity;
            Debug.Log("激光输入, 输入强度:" + inten);
        }

    }
    public override void OnLaserOut(Laser laser)
    {
        if (receivedLasers.Contains(laser))
        {
            laser.UpdateState();
            receivedLasers.Remove(laser);
        }
        inten = 0;
        foreach (var lasers in receivedLasers)
        {
            inten = 0;
            inten += lasers.intensity;
        }
        Debug.Log("激光移除, 现在输入强度:" + inten);

        // 在移除激光后更新状态
        UpdateState();
    }



    public override void DestroyTower()
    {
        base.DestroyTower();
        LightSystem.Instance.RemoveLight(light2d);
    }

    public override void RemoveTower()
    {
        Effect effect = EffectPool.instance.GetObjFromPool(deathEffectName);
        effect.gameObject.transform.position = transform.position;
        LightSystem.Instance.RemoveLight(light2d);
    }
}

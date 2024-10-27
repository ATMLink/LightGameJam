

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AreaLight : Tower
    {
    Light2D light2d;//用来改状态的light2d
    protected bool lightOn = false;
    LightSystem ls;
    float inten;//测试代码,目前不知道为什么所有塔有+400强度bug,则初始化值的时候-400
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
        if (inten > 10f)
            {
            lightOn = true;
            light2d.enabled = true;
            }
        else
            {
            lightOn = false;
            light2d.enabled = false;
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
            inten += lasers.intensity;
            }
        Debug.Log("激光移除, 现在输入强度:" + inten);

        // 在移除激光后更新状态
        UpdateState();
        }
    
   //public override void OnLaserOut(Laser laser)//激光移除时
      //  {
     //   if (receivedLasers.Contains(laser))
     //       {
     //       laser.UpdateState();
      //      receivedLasers.Remove(laser);
      //      }
      //  foreach (var lasers in receivedLasers)
      //      {
     //       inten -= lasers.intensity;
     //       light2d.enabled = false;
     //       }
   //     Debug.Log("激光移除, 现在输入强度:" + inten);
    //    }
    
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

    public override void DestroyTower()
        {
        base.DestroyTower();

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


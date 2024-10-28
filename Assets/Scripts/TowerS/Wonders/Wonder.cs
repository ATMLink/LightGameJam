using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Wonder : Tower
{

    [SerializeField]
    private TowerAttributes destoryAttributes;


    protected virtual void Update()
    {
        //测试用
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    if (destoryAttributes != null)
        //    {
        //        Debug.Log("DestroyTower");
        //        DestroyTower();
        //    }
        //    else
        //    {
        //        Debug.Log("Upgrade");
        //        Upgrade();
        //    }
        //}
    }


    public override void Upgrade()
    {
        if (!LightSystem.Instance.IsIrradiated(transform.position))
        {
            reflectionManager.Reflect("太黑了不能升级");
            return;
        }
        if (attributes.name == "WonderPropeller")
        {
            
            if (resourceManagement.JudgeAfford(element.si, 1000)
                && resourceManagement.JudgeAfford(element.li, 20))
            {
                resourceManagement.SpendResoure(element.si, 1000);
                resourceManagement.SpendResoure(element.li, 20);
                GameObject t2 = Instantiate(attributes.nextLevelAttributes.Prefab, transform.position, Quaternion.identity);
                t2.GetComponent<Wonder>().Initialize();
                Destroy(gameObject);
            }
            else reflectionManager.Reflect("资源不足");
        }
        else if (attributes.name == "WonderSun")
        {
            if (resourceManagement.JudgeAfford(element.si, 5000)
                && resourceManagement.JudgeAfford(element.li, 50)
                && resourceManagement.JudgeAfford(element.cs, 50))
            {
                resourceManagement.SpendResoure(element.si, 5000);
                resourceManagement.SpendResoure(element.li, 50);
                resourceManagement.SpendResoure(element.cs, 50);
                GameObject t2 = Instantiate(attributes.nextLevelAttributes.Prefab, transform.position, Quaternion.identity);
                t2.GetComponent<Wonder>().Initialize();
                Destroy(gameObject);
            }
            else reflectionManager.Reflect("资源不足");
        }
        else if (attributes.name == "WonderLaser")
        {
            if (resourceManagement.JudgeAfford(element.si, 3000)
                && resourceManagement.JudgeAfford(element.cs, 30))
            {
                resourceManagement.SpendResoure(element.si, 3000);
                resourceManagement.SpendResoure(element.cs, 30);
                GameObject t2 = Instantiate(attributes.nextLevelAttributes.Prefab, transform.position, Quaternion.identity);
                t2.GetComponent<Wonder>().Initialize();
                Destroy(gameObject);
            }
            else reflectionManager.Reflect("资源不足");
        }
    }


    public override void Initialize()
    {
        health = attributes.health.Value;
        spriteRenderer.sprite = attributes.towerSprite;
        resourceManagement = GameObject.Find("ResourceManager").GetComponent<MainResourceManagement>();
        reflectionManager = GameObject.Find("ReflectionManager").GetComponent<ReflectionManager>();
        gameObject.SetActive(true);
        material = GetComponent<Renderer>().material;
    }


    public override void DestroyTower()
    {
        GameObject t2 = Instantiate(destoryAttributes.Prefab, transform.position, Quaternion.identity);
        t2.GetComponent<Wonder>().Initialize();
        Destroy(gameObject);
    }

}

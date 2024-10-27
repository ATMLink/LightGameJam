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
        //≤‚ ‘”√
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
        if (attributes.name == "WonderPropeller")
        {
            if (resourceManagement.JudgeAfford(element.si, 1000))
            {
                resourceManagement.SpendResoure(element.si, 1000);
                GameObject t2 = Instantiate(attributes.nextLevelAttributes.Prefab, transform.position, Quaternion.identity);
                t2.GetComponent<Wonder>().Initialize();
                Destroy(gameObject);
            }
        }
        else if (attributes.name == "WonderSun")
        {
            if (resourceManagement.JudgeAfford(element.si, 3000))
            {
                resourceManagement.SpendResoure(element.si, 3000);
                GameObject t2 = Instantiate(attributes.nextLevelAttributes.Prefab, transform.position, Quaternion.identity);
                t2.GetComponent<Wonder>().Initialize();
                Destroy(gameObject);
            }
        }else if (attributes.name == "WonderLaser")
        {
            if (resourceManagement.JudgeAfford(element.si, 2000))
            {
                resourceManagement.SpendResoure(element.si, 2000);
                GameObject t2 = Instantiate(attributes.nextLevelAttributes.Prefab, transform.position, Quaternion.identity);
                t2.GetComponent<Wonder>().Initialize();
                Destroy(gameObject);
            }
        }
    }


    public override void Initialize()
    {

    }


    public override void DestroyTower()
    {
        GameObject t2 = Instantiate(destoryAttributes.Prefab, transform.position, Quaternion.identity);
        t2.GetComponent<Wonder>().Initialize();
        Destroy(gameObject);
    }

}

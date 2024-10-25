using System.Collections;
using System.Collections.Generic;
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
        GameObject t2 = Instantiate(attributes.nextLevelAttributes.Prefab, transform.position, Quaternion.identity);
        t2.GetComponent<Wonder>().Initialize();

        Destroy(gameObject);
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

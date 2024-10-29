using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WonderLaser_Fixed : Wonder
{
    [SerializeField] private LaserForceCollider force;
    [SerializeField] private LaserAt laser;


    private float maxAdvanceCD = 15;
    private float advanceCD = 15;


    protected override void Update()
    {
        UpdateState();
    }

    public override void UpdateState()
    {
        if (advanceCD > 0)
        {
            advanceCD -= Time.deltaTime;
        }
        else
        {
            Launch();
            Debug.Log("Launch!");
            advanceCD = maxAdvanceCD;
        }
    }


    private void Launch()
    {
        force.gameObject.SetActive(true);
    }

    public void Shoot(List<Enemy> enemyList)
    {
        if (enemyList.Count == 0) return;
        Enemy target = null;
        foreach (Enemy enemy in enemyList)
        {
            if(enemy == null || !enemy.gameObject.activeInHierarchy)
            {
                continue;
            }
            if(target == null)
            {
                target = enemy;
            }
            else
            {
                if(target.GetHealthRemain() < enemy.GetHealthRemain())
                {
                    target = enemy;
                }
            }
        }
        if(target != null && target.gameObject.activeInHierarchy)
        {
            StartCoroutine(laser.SetTarget(target));
        }
    }


    public override void Attack()
    {

    }

}

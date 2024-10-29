using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WonderPropeller_Fixed : Wonder
{

    [SerializeField] private PropellerForceCollider force;


    private float maxAdvanceCD = 12;
    private float advanceCD = 12;

    protected override void Update()
    {
        UpdateState();
    }


    public override void UpdateState()
    {
        if(advanceCD > 0)
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





    public override void Attack()
    {

    }

}

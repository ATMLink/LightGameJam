using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WonderPropeller_Fixed : Tower
{

    [SerializeField] private PropellerForceColider force;


    private float maxAdvanceCD = 15;
    private float advanceCD = 15;



    public override void UpdateState()
    {
        if(advanceCD > 0)
        {
            advanceCD -= Time.deltaTime;
        }
        else
        {
            advanceCD = maxAdvanceCD;
        }
    }


    private void Launch()
    {
        force.Launch();
    }





    public override void Attack()
    {

    }

}

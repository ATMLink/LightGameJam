using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : MonoBehaviour
{


    public List<GameObject> towerInSight;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            towerInSight.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null)
        {
            towerInSight.Remove(collision.gameObject);
        }
    }

}

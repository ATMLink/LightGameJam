using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    //������ΪTower
    public List<GameObject> towerInSight;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null)
        {
            if(collision.transform.tag == "Tower")
            {
                towerInSight.Add(collision.gameObject);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision != null)
        {
            if (collision.transform.tag == "Tower")
            {
                towerInSight.Remove(collision.gameObject);
            }
        }
    }
}

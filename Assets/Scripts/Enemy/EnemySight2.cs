using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight2 : MonoBehaviour
{
    public List<Tower> towerInSight;
    public List<Enemy> enemyInSight;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision != null)
        {
            if (collision.transform.tag == "Tower")
            {
                if (!towerInSight.Contains(collision.gameObject.GetComponent<Tower>()))
                {
                    towerInSight.Add(collision.gameObject.GetComponent<Tower>());
                }
            }
            if (collision.transform.tag == "Enemy")
            {
                if (!enemyInSight.Contains(collision.gameObject.GetComponent<Enemy>()))
                {
                    enemyInSight.Add(collision.gameObject.GetComponent<Enemy>());
                }
            }
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision != null)
        {
            if (collision.transform.tag == "Tower")
            {
                if (!towerInSight.Contains(collision.gameObject.GetComponent<Tower>()))
                {
                    towerInSight.Add(collision.gameObject.GetComponent<Tower>());
                }
            }
            if (collision.transform.tag == "Enemy")
            {
                if (!enemyInSight.Contains(collision.gameObject.GetComponent<Enemy>()))
                {
                    enemyInSight.Add(collision.gameObject.GetComponent<Enemy>());
                }
            }
        }
    }
}

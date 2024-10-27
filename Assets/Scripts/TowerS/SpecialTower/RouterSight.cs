using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RouterSight : MonoBehaviour
{

    public List<Tower> towerInSight = new List<Tower>();

    public void Clear()
    {
        towerInSight.Clear();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.transform.tag == "Tower")
            {
                Tower tower = collision.GetComponent<Tower>();
                if (!towerInSight.Contains(tower))
                {
                    towerInSight.Add(collision.gameObject.GetComponent<Tower>());
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.transform.tag == "Tower")
            {
                towerInSight.Remove(collision.gameObject.GetComponent<Tower>());
            }
        }
    }

}

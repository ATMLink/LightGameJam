using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    public List<Tower> towerInSight;


    public void Refresh()
    {
        List<Tower> list = new List<Tower>();
        foreach (var tower in towerInSight)
        {
            if (!tower.gameObject.activeInHierarchy)
            {
                Debug.Log("ÒÆ³ý");
                list.Add(tower);
            }
        }
        foreach (var tower in list)
        {
            towerInSight.Remove(tower);
        }
    }

    public void Clear()
    {
        towerInSight.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.transform.tag == "Tower")
            {
                towerInSight.Add(collision.gameObject.GetComponent<Tower>());
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

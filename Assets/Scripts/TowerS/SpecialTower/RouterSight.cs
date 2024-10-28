using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RouterSight : MonoBehaviour
{

    public List<Tower> towerInSight = new List<Tower>();
    [SerializeField]
    private RouterTower router;

    //public void ResetList()
    //{
    //    List<Tower> tempList = new List<Tower>(towerInSight);
    //    foreach (Tower tower in tempList)
    //    {

    //    }
    //}

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
                Tower tower = collision.gameObject.GetComponent<Tower>();
                towerInSight.Remove(tower);
                router.LeftTower(tower);
            }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSight : MonoBehaviour
{
    public List<Enemy> EnemyInSight;

    public void Refresh()
    {
        List<Enemy> list = new List<Enemy>();
        foreach (var enemy in EnemyInSight)
        {
            if (!enemy.gameObject.activeInHierarchy)
            {
                list.Add(enemy);
            }
        }
        foreach (var enemy in list)
        {
            EnemyInSight.Remove(enemy);
        }
    }

    public void Clear()
    {
        EnemyInSight.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.transform.tag == "Enemy")
            {
                EnemyInSight.Add(collision.gameObject.GetComponent<Enemy>());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.transform.tag == "Enemy")
            {
                EnemyInSight.Remove(collision.gameObject.GetComponent<Enemy>());
            }
        }
    }
}

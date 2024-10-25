using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnemyTest : MonoBehaviour
{


    //public List<GameObject> towerInSight;

    public new Light2D light;


    private void Start()
    {
        LightSystem.Instance.AddLight(light);
    }

    private void Update()
    {
        //Debug.Log(LightSystem.Instance.IsIrradiated(gameObject.transform.position));
        //if (LightSystem.Instance.IsIrradiated(gameObject.transform.position))
        //{
        //    Debug.Log("lighing");
        //}
        //else
        //{
        //    Debug.Log("dark");
        //}
    }


    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision != null)
    //    {
    //        towerInSight.Add(collision.gameObject);
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision != null)
    //    {
    //        towerInSight.Remove(collision.gameObject);
    //    }
    //}

}

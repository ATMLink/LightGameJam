using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using static UnityEngine.ParticleSystem;

public class LaserForceCollider : MonoBehaviour
{

    public List<Enemy> EnemyInSight = new List<Enemy>();

    [SerializeField]
    private ParticleSystem particle;

    [SerializeField]
    private WonderLaser_Fixed wonder;

    [SerializeField]
    private new CircleCollider2D collider;

    private float maxCircleRadius = 40;
    private float minCircleRadius = 0.5f;
    private float currentRadius = 0.5f;

    private float maxGeSpeed = 12f;
    private float minGeSpeed = 3f;
    private float geSpeed = 3f;


    private void OnEnable()
    {
        EnemyInSight .Clear();
        currentRadius = minCircleRadius;
        geSpeed = minGeSpeed;
        StartCoroutine(Launch());
    }

    private void OnDisable()
    {

    }


    private IEnumerator Launch()
    {
        ShapeModule shape = particle.shape;
        float runTime = 0;
        while (true)
        {
            runTime += Time.deltaTime;
            currentRadius += geSpeed * Time.deltaTime;
            collider.radius = currentRadius / 3;
            shape.radius = currentRadius;
            var emission = particle.emission;
            emission.rateOverTime = 2 * currentRadius * Mathf.PI * 400;
            if (geSpeed < maxGeSpeed) geSpeed = minGeSpeed + pow2(runTime) * maxCircleRadius;
            if (currentRadius > maxCircleRadius)
            {
                break;
            }
            yield return null;
        }

        wonder.Shoot(EnemyInSight);
        gameObject.SetActive(false);
        yield return null;
    }


    private float pow2(float x)
    {
        if (x >= 1) return 1;
        return x * x;
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

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision != null)
    //    {
    //        if (collision.transform.tag == "Enemy")
    //        {
    //            EnemyInSight.Remove(collision.gameObject.GetComponent<Enemy>());
    //        }
    //    }
    //}
}

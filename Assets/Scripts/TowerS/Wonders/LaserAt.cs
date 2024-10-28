using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LaserAt : MonoBehaviour
{

    public List<Enemy> EnemyInSight = new List<Enemy>();

    [SerializeField]
    private CircleCollider2D circleCollider;
    [SerializeField]
    private new Light2D light;


    private float maxPreTime = 1f;
    private float maxEndTime = 1f;
    private float maxIntensity = 0.8f;
    private float minIntensity = 0f;



    [SerializeField]
    private int maxHitCount = 5;

    [SerializeField]
    private float LaserDamage = 24;
    private float attackCD = 1f;



    public IEnumerator SetTarget(Enemy enemy)
    {
        EnemyInSight.Clear();
        light.enabled = true;


        float timer = 0;
        while (timer < maxPreTime)
        {
            timer += Time.deltaTime;
            if (enemy.gameObject.activeInHierarchy)
            {
                gameObject.transform.position = enemy.transform.position;
            }

            float t = Mathf.Clamp01(timer / maxPreTime);
            light.intensity = Mathf.Lerp(minIntensity, maxIntensity, t);
            yield return null;
        }
        light.intensity = maxIntensity;

        circleCollider.enabled = true;

        timer = 0;
        int hitCount = 0;

        while (true)
        {
            if (enemy.gameObject.activeInHierarchy)
            {
                gameObject.transform.position = enemy.transform.position;
            }
            //¸üÐÂÎ»ÖÃ£¬ÔÚ¹Ö´æ»îÊ±¸úËæ¹¥»÷£¬¹ÖËÀÍöºóÍ£Ö¹¸úËæ

            timer += Time.deltaTime;
            if(timer > attackCD)
            {
                timer = 0;
                if (hitCount > maxHitCount) break;
                List<Enemy> enemySaving = new List<Enemy>(EnemyInSight);
                foreach (var en in enemySaving)
                {
                    if (en.gameObject.activeInHierarchy)
                    {
                        en.OnHit(LaserDamage);
                    }
                }
                hitCount++;
            }


            yield return null;
        }

        timer = 0;
        while (timer < maxEndTime)
        {
            timer += Time.deltaTime;
            if (enemy.gameObject.activeInHierarchy)
            {
                gameObject.transform.position = enemy.transform.position;
            }

            float t = Mathf.Clamp01(timer / maxEndTime);
            light.intensity = Mathf.Lerp(maxIntensity, minIntensity, t);
            yield return null;
        }
        light.intensity = minIntensity;

        circleCollider.enabled = false;
        light.enabled = false;
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

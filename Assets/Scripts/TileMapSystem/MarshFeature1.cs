using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MountainFeature : TilemapFeature
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && !canEnemyThrough)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if (enemy != null)
            {
                Debug.LogWarning(111);
                rb.velocity = rb.velocity + new Vector2((collision.transform.position - transform.position).x, (collision.transform.position - transform.position).y);
                //enemy.SetSpeed(-1);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && !canEnemyThrough) {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.SetSpeed(1f);
            }
        }
    }

}

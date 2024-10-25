using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProK : Projectile
{

    public List<Enemy> EnemyInSight;

    protected float straightDamage = 0;

    protected override IEnumerator MoveToTarget(float damage)
    {
        straightDamage = damage;
        Vector3 origin = transform.position;
        while (true)
        {
            if (target != null)
            {
                targetpos = target.position;
                // 计算目标与当前物体之间的方向
                direction = (target.position - transform.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
                target = null;
            }

            transform.position += direction * speed * Time.deltaTime; // 移动子弹

            // 确保投射物到达目标位置
            if (Vector3.Distance(transform.position, origin) > destoryDistance)
            {
                ReturnToPool();
                break;
            }

            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.transform.tag == "Enemy" && !EnemyInSight.Contains(collision.gameObject.GetComponent<Enemy>()))
            {
                Debug.Log(straightDamage);
                HitTarget(straightDamage); // 击中目标
                EnemyInSight.Add(collision.gameObject.GetComponent<Enemy>());
            }
        }
    }

    protected override void ReturnToPool()
    {
        EnemyInSight.Clear();
        base.ReturnToPool();
    }

}

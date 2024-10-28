using System.Collections;
using UnityEngine;

public class proNa : Projectile
{

    protected override IEnumerator MoveToTarget(float damage)
    {
        if (target != null)
        {
            targetpos = target.position;
            // 计算目标与当前物体之间的方向
            direction = (target.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        }
        while (true)
        {

            transform.position += direction * speed * Time.deltaTime; // 移动子弹

            // 确保投射物到达目标位置
            if (Vector3.Distance(transform.position, targetpos) < destoryDistance)
            {
                if (target.gameObject.activeInHierarchy)
                {
                    HitTarget(damage); // 击中目标
                }
                ReturnToPool();
                break;
            }

            yield return null;
        }
    }

    protected override void HitTarget(float damage)
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, 0.5f);
        Enemy mainEnemy = null;
        if (target != null && target.gameObject.activeInHierarchy)
        {
            mainEnemy = target.GetComponent<Enemy>();
        }

        foreach (var enemyCollider in enemies)
        {
            Enemy enemy = enemyCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (enemy == mainEnemy)
                {
                    enemy.OnHit(damage + 10);
                }
                else
                {
                    enemy.OnHit(damage);
                }
            }

        }

        Effect effect = EffectPool.instance.GetObjFromPool("NaBoomEffect");
        effect.gameObject.transform.position = transform.position;


        Destroy(gameObject);
    }


}

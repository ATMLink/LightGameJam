using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public string projectileName;
    [SerializeField]
    protected float speed = 10f;

    protected Transform target;
    protected Vector3 targetpos;
    protected Vector3 direction;

    [SerializeField]
    protected float destoryDistance = 0.2f;

    public void Launch(Transform targetTransform, float damage)
    {
        target = targetTransform;
        StartCoroutine(MoveToTarget(damage));
    }

    protected virtual IEnumerator MoveToTarget(float damage)
    {
        while (true)
        {
            if (target != null)
            {
                targetpos = target.position;
                // 计算目标与当前物体之间的方向
                direction = (target.position - transform.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
            }

            transform.position += direction * speed * Time.deltaTime; // 移动子弹

            // 确保投射物到达目标位置
            if (Vector3.Distance(transform.position, targetpos) < destoryDistance)
            {
                if (target.gameObject.activeInHierarchy)
                {
                    transform.position = target.position;
                    HitTarget(damage); // 击中目标
                }
                ReturnToPool();
                break;
            }

            yield return null;
        }
    }

    protected virtual void HitTarget(float damage)
    {
        
    }

    protected virtual void ReturnToPool()
    {
        ProjectilePool.instance.ReturnObjToPool(this, projectileName);
    }
}

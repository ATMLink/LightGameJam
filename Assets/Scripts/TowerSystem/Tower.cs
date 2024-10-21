using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public TowerAttributes attributes;

    private float health;
    private float damage;
    private float attackSpeed;
    private float attackRange;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private float attackCooldown;
    private float attackTimer;
    
    private List<Enemy> enemiesInRange;
    [SerializeField] private CircleCollider2D rangeCollider;

    public void Initialize()
    {
        health = attributes.health.Value;
        damage = attributes.damage.Value;
        attackSpeed = attributes.attackSpeed.Value;
        attackRange = attributes.attackRange.Value;
        
        // set tower sprite
        // spriteRenderer = GetComponent<SpriteRenderer>();
        // if (spriteRenderer == null)
        //     spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = attributes.towerSprite;

        enemiesInRange = new List<Enemy>();

        attackCooldown = 1f / attackSpeed;
        attackTimer = 0f;

        // set tower circle collider
        // rangeCollider = GetComponent<CircleCollider2D>();
        // if (rangeCollider == null)
        //     rangeCollider = gameObject.AddComponent<CircleCollider2D>();
        // rangeCollider.isTrigger = true;
        rangeCollider.radius = attackRange;
        
        gameObject.SetActive(true);
    }

    public void RenderPreview()
    {
        spriteRenderer.sprite = attributes.towerSprite;
    }
    
    // 重置塔的属性，方便对象池回收
    public void ResetAttributes()
    {
        health = 0;
        damage = 0;
        attackSpeed = 0;
        attackRange = 0;
        enemiesInRange.Clear();
    }

    public void Upgrade()
    {
        if (attributes.nextLevelAttributes != null)
        {
            attributes = attributes.nextLevelAttributes;
            Initialize();
        }
        else
        {
            Debug.Log("已经达到最高等级，无法继续升级。");
        }
    }

    public void DestroyTower()
    {
        gameObject.SetActive(false); // 将塔移回对象池
    }

    public void Attack()
    {
        if (attackTimer >= attackCooldown && enemiesInRange.Count > 0)
        {
            // 攻击最近的敌人
            Enemy target = FindClosestEnemy();
            if (target != null)
            {
                target.OnHit(damage); // 对敌人造成伤害
                attackTimer = 0f; // 重置攻击计时器
            }
        }
        else
        {
            attackTimer += Time.deltaTime; // 增加计时器
        }
    }

    public void OnHit(int damage)
    {
        if(health - damage <= 0)
            DestroyTower();
        health -= damage;
    }
    
    private Enemy FindClosestEnemy()
    {
        Enemy closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (Enemy enemy in enemiesInRange)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null)
        {
            Debug.LogError("Collision is null in OnTriggerEnter2D.");
            return;
        }
        if (collision.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemiesInRange.Add(enemy); // 添加进入范围的敌人
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == null)
        {
            Debug.LogError("Collision is null in OnTriggerExit2D.");
            return;
        }
        if (collision.TryGetComponent<Enemy>(out Enemy enemy))
        {
            //enemiesInRange.Remove(enemy); // 移除离开范围的敌人
            if (enemy != null && !enemy.Equals(null)) // 确保 enemy 没有被销毁
            {
                enemiesInRange.Remove(enemy);
                Debug.Log($"Enemy {enemy.name} removed from range.");
            }
        }
    }
}

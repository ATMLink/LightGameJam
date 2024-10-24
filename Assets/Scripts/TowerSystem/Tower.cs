using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private Laser attachedLaser;
    
    public TowerAttributes attributes;

    public int towerID;
    private static int towerIDCounter = 0;
    private float health;
    private float damage;
    private float attackSpeed;
    private float attackRange;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private float attackCooldown;
    private float attackTimer;
    
    private List<Enemy> enemiesInRange;

    [SerializeField] private TowerSight sight1;


    // private void Start()
    // {
    //     sight1 = transform.GetChild(0).GetComponent<TowerSight>();
    // }

    public virtual void Initialize()
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

        transform.rotation = Quaternion.Euler(Vector3.down);

        // set tower circle collider
        // rangeCollider = GetComponent<CircleCollider2D>();
        // if (rangeCollider == null)
        //     rangeCollider = gameObject.AddComponent<CircleCollider2D>();
        // rangeCollider.isTrigger = true;
       sight1.GetComponent<CircleCollider2D>().radius = attackRange;
        
       towerID = towerIDCounter++;
        
        gameObject.SetActive(true);
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
        sight1.Clear();
        gameObject.SetActive(false); // 将塔移回对象池
    }

    public virtual void Attack()
    {
        if (enemiesInRange.Count > 0 && attackTimer >= attackCooldown)
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

    public virtual bool OnLaserHit(Laser laser)
    {
        return false;
    }
    
    public void OnHit(int damage)
    {
        health -= damage;
        if (health <= 0)DestroyTower();
    }
    
    private Enemy FindClosestEnemy()
    {
        sight1.Refresh();

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

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision == null)
    //    {
    //        Debug.LogError("Collision is null in OnTriggerEnter2D.");
    //        return;
    //    }
    //    if (collision.TryGetComponent<Enemy>(out Enemy enemy))
    //    {
    //        enemiesInRange.Add(enemy); // 添加进入范围的敌人 
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision == null)
    //    {
    //        Debug.LogError("Collision is null in OnTriggerExit2D.");
    //        return;
    //    }
    //    if (collision.TryGetComponent<Enemy>(out Enemy enemy))
    //    {
    //        //enemiesInRange.Remove(enemy); // 移除离开范围的敌人
    //        if (enemy != null && !enemy.Equals(null)) // 确保 enemy 没有被销毁
    //        {
    //            enemiesInRange.Remove(enemy);
    //            Debug.Log($"Enemy {enemy.name} removed from range.");
    //        }
    //    }
    //}
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    protected Laser attachedLaser;
    
    public TowerAttributes attributes;

    public int towerID;
    protected static int towerIDCounter = 0;
    
    protected float health;
    protected float damage;
    protected float attackSpeed;
    protected float attackRange;
    protected float attackCooldown;
    protected float attackTimer;
    
    protected List<Laser> receivedLasers;
    private List<Enemy> enemiesInRange;
    
    [SerializeField] private SpriteRenderer spriteRenderer;
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
        spriteRenderer.sprite = attributes.towerSprite;

        enemiesInRange = new List<Enemy>();
        receivedLasers = new List<Laser>();

        attackCooldown = 1f / attackSpeed;
        attackTimer = 0f;

        transform.rotation = Quaternion.Euler(Vector3.down);
        
        sight1.GetComponent<CircleCollider2D>().radius = attackRange;
        
        towerID = towerIDCounter++;
        
        gameObject.SetActive(true);
    }
    
    public virtual void UpdateState()
    {
        // Attack();
    }
    
    // 重置塔的属性，方便对象池回收
    public virtual void ResetAttributes()
    {
        health = 0;
        damage = 0;
        attackSpeed = 0;
        attackRange = 0;
        enemiesInRange.Clear();
        receivedLasers.Clear();
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

    public virtual List<Laser> OnLaserHit(Laser laser)
    {
        Debug.Log($"{gameObject.name} 被激光击中了");
        // receivedLasers.Add(laser);
        // return receivedLasers;
        return receivedLasers;
    }
    
    
    public virtual void OnHit(int damage)
    {
        health -= damage;
        if (health <= 0)DestroyTower();
    }

    public virtual List<Laser> OnLaserOut(Laser laser)
    {
        Debug.Log($"{gameObject.name} 离开了");
        return null;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 检查碰撞的对象是否是 Laser，并且是否带有 "Laser" 标签
        if (collision.CompareTag("Laser"))
        {
            Laser laser = collision.GetComponent<Laser>();
            if (laser != null)
            {
                // 调用塔的 OnLaserHit 方法处理激光击中
                OnLaserHit(laser);
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Laser"))
        {
            Laser laser = collision.GetComponent<Laser>();
            if (laser != null)
            {
                // 调用塔的 OnLaserHit 方法处理激光击中
                OnLaserOut(laser);
            }
        }
    }
    
}

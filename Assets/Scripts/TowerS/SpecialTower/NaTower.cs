using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaTower : TowerProjectile
    {
    private const float laserIntensityThreshold = 30f;
    private const float attackRadius = 3.5f;
    private const float explosionRadius = 1.5f;
    private const float damageValue = 10f;
    private const float attackInterval = 1.8f;
    private float attackTimer = 0f;

    [SerializeField] private GameObject bombPrefab; // 设置炸弹预制体
    [SerializeField] private ParticleSystem explosionEffect; // 爆炸效果

    private void Start()
        {
        attackCooldown = attackInterval;
        sight1.GetComponent<CircleCollider2D>().radius = attackRadius;
        }

    public override void UpdateState()
        {
        base.UpdateState();
        if (receivedLasers != null)
            {
            float totalIntensity = 0;
            foreach (var laser in receivedLasers)
                {
                totalIntensity += laser.intensity;
                }
            canAttack = totalIntensity >= laserIntensityThreshold;
            }
        Attack();
        }

    public override void Attack()
        {
        if (!canAttack || sight1.EnemyInSight.Count == 0 || attackTimer < attackCooldown) return;

        Enemy target = FindClosestEnemy();
        if (target != null && target.gameObject.activeInHierarchy)
            {
            LaunchBomb(target);
            attackTimer = 0f;
            }
        else
            {
            attackTimer += Time.deltaTime;
            }
        }

    private void LaunchBomb(Enemy target)
        {
        GameObject bomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);
        Bomb bombComponent = bomb.GetComponent<Bomb>();
        bombComponent.Initialize(target.transform, damageValue, explosionRadius, explosionEffect);
        }

    public override void OnLaserHit(Laser laser)
        {
        base.OnLaserHit(laser);
        CheckLaserIntensity();
        }

    public override void OnLaserOut(Laser laser)
        {
        base.OnLaserOut(laser);
        CheckLaserIntensity();
        }

    private void CheckLaserIntensity()
        {
        float totalIntensity = 0;
        foreach (var laser in receivedLasers)
            {
            totalIntensity += laser.intensity;
            }
        canAttack = totalIntensity >= laserIntensityThreshold;
        }
    }
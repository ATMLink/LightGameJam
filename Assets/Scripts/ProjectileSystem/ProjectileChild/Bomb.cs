using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
    {
    private Transform target;
    private float damage;
    private float explosionRadius;
    private ParticleSystem explosionEffect;

    public void Initialize(Transform target, float damage, float radius, ParticleSystem effect)
        {
        this.target = target;
        this.damage = damage;
        this.explosionRadius = radius;
        this.explosionEffect = effect;
        }

    private void Update()
        {
        if (target == null) return;

        transform.position = Vector3.MoveTowards(transform.position, target.position, 10 * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
            {
            Explode();
            }
        }

    private void Explode()
        {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (var enemyCollider in enemies)
            {
            Enemy enemy = enemyCollider.GetComponent<Enemy>();
            if (enemy != null)
                {
                enemy.OnHit(damage);
                }
            }

        if (explosionEffect != null)
            {
            ParticleSystem effect = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            effect.Play();
            }

        Destroy(gameObject);
        }
    }

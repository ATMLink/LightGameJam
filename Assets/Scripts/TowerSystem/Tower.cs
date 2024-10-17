using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public FloatVariable health;
    public FloatVariable damage;
    public FloatVariable shootSpeed;
    public FloatVariable range;  // 攻击范围
    public bool needsLaser;
    public FloatVariable laserPower;

    //private Laser laser;  // 如果塔使用激光

    public void Initialize(Vector3 position, FloatVariable health, FloatVariable damage, FloatVariable shootSpeed, FloatVariable range, bool needsLaser, FloatVariable laserPower)
    {
        this.transform.position = position;
        this.health = health;
        this.damage = damage;
        this.shootSpeed = shootSpeed;
        this.range = range;
        this.needsLaser = needsLaser;
        this.laserPower = laserPower;

        if (needsLaser)
        {
      //      laser = new Laser();
            // 初始化激光
        }
    }

    public void Upgrade()
    {
        // 提升塔的属性，如增加攻击力、范围等
        // damage.Value += 10;
        // health.Value += 20;
        // Debug.Log("Tower upgraded.");
    }

    public void DestroyTower()
    {
        Destroy(gameObject);
        Debug.Log("Tower destroyed.");
    }
}

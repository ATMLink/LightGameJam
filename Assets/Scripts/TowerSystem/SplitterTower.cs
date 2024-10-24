using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitterTower : Tower
{
    public int numberOfLasers = 3;
    public float angleBetweenLasers = 30f;

    // 重写 OnLaserHit 方法，处理激光击中
    public override bool OnLaserHit(Laser laser)
    {
        Vector3 hitPosition = laser.transform.position;
        Vector3 laserDirection = laser.transform.right; // 获取激光的方向
        Debug.Log($"{gameObject.name} 被激光击中，并生成了 {numberOfLasers} 条激光。");
        return false;
    }
}

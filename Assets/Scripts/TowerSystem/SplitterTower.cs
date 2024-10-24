using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitterTower : Tower
{
    public int numberOfLasers = 3;
    public float angleBetweenLasers = 30f;

    // ��д OnLaserHit ���������������
    public override bool OnLaserHit(Laser laser)
    {
        Vector3 hitPosition = laser.transform.position;
        Vector3 laserDirection = laser.transform.right; // ��ȡ����ķ���
        Debug.Log($"{gameObject.name} ��������У��������� {numberOfLasers} �����⡣");
        return false;
    }
}

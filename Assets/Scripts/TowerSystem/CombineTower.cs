using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineTower : Tower
{
    public int maxLasers = 7; // �Ϲ���������7������
    public Vector3 blockedDirection = Vector3.down; // ���岻�ܽ��ܼ���ķ�����������Ҳ಻�ܽ��ܣ�

    private float totalIntensity = 0;
    private float maxTotalIntensity = 200f; // �Ϲ�������󼤹�ǿ��
    private float updateInterval = 0.1f; // ÿ��0.1�����һ�μ���ǿ��
    private float lastUpdateTime = 0f;

    private LaserManager laserManager;
    private Vector3 outputDirection = Vector3.up; // �Ϲ����������ķ���
    
    public override void Initialize()
    {
        base.Initialize();
        laserManager = FindObjectOfType<LaserManager>();
        // �Ϲ�����ʼ�����������⣬�ȴ����յ��㹻�����뼤���ſ�ʼ���
    }

    public override void UpdateState()
    {
        // ���¼���ǿ��
        if (Time.time - lastUpdateTime >= updateInterval)
        {
            UpdateLaserIntensity();
            lastUpdateTime = Time.time;
        }
    }

    public override void ResetAttributes()
    {
        base.ResetAttributes();
        totalIntensity = 0;
        lastUpdateTime = 0f;
    }

    // ��д OnLaserHit �������������뼤��
    public override List<Laser> OnLaserHit(Laser laser)
    {
        // �жϼ��ⷽ���Ƿ����� blockedDirection��������ܾ�����
        if (Vector3.Dot(laser.direction, blockedDirection.normalized) > 0.99f)
        {
            Debug.Log("Blocked laser from direction: " + laser.direction);
            return receivedLasers;
        }

        if (receivedLasers.Count < maxLasers)
        {
            if (!receivedLasers.Contains(laser))
            {
                receivedLasers.Add(laser);
                Debug.Log("Received laser from direction: " + laser.direction);
            }
        }

        return receivedLasers;
    }

    public override List<Laser> OnLaserOut(Laser laser)
    {
        if (receivedLasers.Contains(laser))
        {
            receivedLasers.Remove(laser);
            return receivedLasers;
        }

        return null;
    }

    // ���¼���ǿ�Ȳ�����ϲ���ļ���
    public void UpdateLaserIntensity()
    {
        totalIntensity = 0;

        // ������յ������м������ǿ��
        foreach (var receivedLaser in receivedLasers)
        {
            totalIntensity += receivedLaser.intensity;
        }

        // ������ǿ�Ȳ��ܳ������ֵ
        if (totalIntensity > maxTotalIntensity)
        {
            totalIntensity = maxTotalIntensity;
        }

        // ����н��յ����⣬����ϲ���ļ���
        if (receivedLasers.Count > 0)
        {
            // �������Ƿ��Ѿ���������⣬���û���򴴽�һ���µ�
            List<Laser> emittedLasers = laserManager.GetLaserForTower(this);
            if (emittedLasers == null || emittedLasers.Count == 0)
            {
                laserManager.CreateLaser(this, transform.position, outputDirection, totalIntensity);
            }
            else
            {
                // ����Ѿ���������⣬�������ǿ��
                foreach (var laser in emittedLasers)
                {
                    laser.SetLaserProperties(totalIntensity, outputDirection);
                }
            }
        }
    }
}

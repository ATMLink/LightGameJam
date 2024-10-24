using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitterTower : Tower
{
    public int numberOfLasers = 2;
    // public float decay = 0.5f;
    private float totalIntensity = 0;
    private float maxTotalIntensity = 100f;
    private float updateInterval = 0.1f; // ÿ��0.1�����һ�μ���ǿ��
    private float lastUpdateTime = 0f;

    private LaserManager laserManager;

    public override void Initialize()
    {
        base.Initialize();
        laserManager = FindObjectOfType<LaserManager>();
    }

    public override void UpdateState()
    {
        // ���㲢����ǿ��
        if (Time.time - lastUpdateTime >= updateInterval)
        {
            UpdateLaserIntensity();
            lastUpdateTime = Time.time; // ��¼��һ�θ��µ�ʱ��
        }
    }

    public override void ResetAttributes()
    {
        base.ResetAttributes();
        totalIntensity = 0;
        lastUpdateTime = 0f;
    }

    // ��д OnLaserHit ����
    public override List<Laser> OnLaserHit(Laser laser)
    {
        if (receivedLasers.Count <= 0)
        {
            receivedLasers.Add(laser);
        }

        if (receivedLasers.Count == 1)
        {
            Vector3 originalDirection = laser.direction;

            Vector3 referenceVector = Vector3.up;

            // ��ԭʼ���ⴹֱ����������
            Vector3 outLaserDirection1 = Vector3.Cross(originalDirection, referenceVector).normalized;
            Vector3 outLaserDirection2 = Vector3.Cross(originalDirection, outLaserDirection1).normalized;
            
            // ��������
            laserManager.CreateLaser(this, transform.position,
                outLaserDirection1, laser.intensity / 2);
            laserManager.CreateLaser(this, transform.position,
                outLaserDirection2, laser.intensity / 2);
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
    

    public void UpdateLaserIntensity()
    {
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
        
        // ����ֹ��������ļ���ǿ�ȣ������¼�������
        float emittedIntensity = totalIntensity / numberOfLasers;

        // ��ȡ����������ļ��Ⲣ������ǿ��
        List<Laser> emittedLasers = laserManager.GetLaserForTower(this);
        if (emittedLasers != null)
        {
            foreach (var laser in emittedLasers)
            {
                laser.SetLaserProperties(emittedIntensity, laser.direction);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �����ײ�Ķ����Ƿ��� Laser�������Ƿ���� "Laser" ��ǩ
        if (collision.CompareTag("Laser"))
        {
            Laser laser = collision.GetComponent<Laser>();
            if (laser != null)
            {
                // �������� OnLaserHit �������������
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
                // �������� OnLaserHit �������������
                OnLaserOut(laser);
            }
        }
    }
}

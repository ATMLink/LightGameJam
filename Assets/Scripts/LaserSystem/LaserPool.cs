using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPool : MonoBehaviour
{
    public GameObject laserPrefab; // ����Ԥ����
    public int initialPoolSize = 8; // ��ʼ����صĴ�С
    private List<Laser> laserPool; // �����

    public void Initialize()
    {
        laserPool = new List<Laser>();

        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject laserObject = Instantiate(laserPrefab);
            Laser laser = laserObject.GetComponent<Laser>();
            laserPool.Add(laser);
            laser.SetLaserActive(false); // ��ʼ��ʱ���ؼ���
        }
    }

    public Laser GetLaser(Vector3 position, Vector3 direction, float intensity)
    {
        foreach (Laser laser in laserPool)
        {
            if (!laser.gameObject.activeInHierarchy) // �������δ���������
            {
                laser.transform.position = position;
                laser.SetLaserProperties(intensity, direction);
                laser.SetLaserActive(true); // �����
                return laser;
            }
        }

        // ���û�п��õļ��⣬���ݳ�
        ExpandPool(1);
        return GetLaser(position, direction, intensity); // �ݹ�����Ի�ȡ�µļ���
    }

    private void ExpandPool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject laserObject = Instantiate(laserPrefab);
            Laser laser = laserObject.GetComponent<Laser>();
            laser.SetLaserActive(false); // ��ʼ��ʱ���ؼ���
            laserPool.Add(laser);
        }

        Debug.Log($"Laser pool expanded. New size: {laserPool.Count}");
    }

    public void ReturnLaser(Laser laser)
    {
        laser.ResetLaser();
        laser.SetLaserActive(false); // ���ؼ���
    }
}

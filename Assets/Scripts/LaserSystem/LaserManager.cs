using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserManager : MonoBehaviour
{
    public LaserPool laserPool; // ���������
    private List<Laser> activeLasers = new List<Laser>(); // �洢��ǰ����ļ���

    public void Initialize()
    {
        laserPool.Initialize();
    }

    public void UpdateState()
    {
        // �������м���ļ���״̬
        foreach (Laser laser in activeLasers)
        {
            laser.UpdateState();
        }
    }

    /// <summary>
    /// Create a laser with parameters position, direction, intensity
    /// </summary>
    /// <param name="position"></param>
    /// <param name="direction"></param>
    /// <param name="intensity"></param>
    /// <returns></returns>
    public Laser CreateLaser(Vector3 position, Vector3 direction, float intensity)
    {
        // �Ӷ���ػ�ȡ����
        Laser laser = laserPool.GetLaser(position, direction, intensity);
        if (laser != null)
        {
            activeLasers.Add(laser);
            laser.Initialize();
            laser.SetLaserActive(true);
        }
        return laser;
    }

    public void RemoveLaser(Laser laser)
    {
        laserPool.ReturnLaser(laser); // �����ⷵ�س���
        activeLasers.Remove(laser); // �Ӽ����б����Ƴ�����
    }
}

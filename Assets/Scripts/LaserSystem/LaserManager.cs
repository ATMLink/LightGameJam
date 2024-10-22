using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserManager : MonoBehaviour
{
    public LaserPool laserPool; // ���������
    private Dictionary<Tower, Laser> towerLaserMap = new Dictionary<Tower, Laser>(); // �洢���뼤���ӳ���ϵ

    public void Initialize()
    {
        laserPool.Initialize();
    }

    public void UpdateState()
    {
        // �������м���ļ���״̬
        foreach (Laser laser in towerLaserMap.Values)
        {
            laser.UpdateState();
        }
    }

    /// <summary>
    /// ������������ͼ���
    /// </summary>
    /// <param name="position"></param>
    /// <param name="direction"></param>
    /// <param name="intensity"></param>
    /// <returns></returns>
    public Laser CreateLaser(Tower tower, Vector3 position, Vector3 direction, float intensity)
    {
        // �Ӷ���ػ�ȡ����
        Laser laser = laserPool.GetLaser(position, direction, intensity);
        if (laser != null)
        {
            towerLaserMap[tower] = laser; // �����ͼ���
            laser.Initialize();
            laser.SetLaserActive(true);
        }
        return laser;
    }

    public void RemoveLaser(Tower tower)
    {
        if (towerLaserMap.ContainsKey(tower))
        {
            Laser laser = towerLaserMap[tower];
            laserPool.ReturnLaser(laser); // �����ⷵ�س���
            towerLaserMap.Remove(tower); // ��ӳ���ϵ���Ƴ�����
        }
    }
    
    /// <summary>
    /// ��ȡָ�����ļ���
    /// </summary>
    public Laser GetLaserForTower(Tower tower)
    {
        if (towerLaserMap.ContainsKey(tower))
        {
            return towerLaserMap[tower];
        }
        return null;
    }
    
    /// <summary>
    /// ����ָ�����ļ���ļ���״̬
    /// </summary>
    public void SetLaserActiveForTower(Tower tower, bool isActive)
    {
        Laser laser = GetLaserForTower(tower);
        if (laser != null)
        {
            laser.SetLaserActive(isActive);
        }
    }
}

using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// 核心塔的子类,决定了核心塔的行为, 包括激光强度
/// </summary>
public class CoreTower : Tower
    {
    private Vector3 laserDirection = Vector3.down; // 设置激光初始方向为下
    public GameDriver gameDriver;

    private void Start()//取得用来结束游戏的gamedirver
        {
        GameObject temp = GameObject.Find("GameDriver");
        gameDriver = temp.GetComponent<GameDriver>();
        }

    public override void OnHit(int damage)
        {
        base.OnHit(damage);
        if (health <= 0) { gameDriver.EndGame(); }//血量为0结束游戏
        }
    public override void Upgrade()
        {
        base.Upgrade();
        Tower tower = GetComponent<Tower>(); //获取tower脚本
        laserManager.RemoveLaser(tower);//删除旧激光
        laserManager.CreateLaser(tower, transform.position, Vector3.down, 1000);//创建新激光

        }

    }






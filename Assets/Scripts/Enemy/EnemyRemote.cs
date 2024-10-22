using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRemote : Enemy
{

    protected float maxAttackCost = 0;//����ʱ��ͣ��
    protected float attackCost = 0;
    protected List<Tower> towerInBlock = new List<Tower>();

    private bool onBlock = false;


    //


    //ע��:����Ŀǰ�ز�����,�����ֵ��˽������ŴӶ��ı��С,��˵��˵���Ұ��ײ��Ҳ������Ӧ����


    //


    protected override void CopyData()
    {
        base.CopyData();
        maxAttackCost = saving.specialAbility[0];
    }


    #region update
    protected override void AttackPlan()
    {
        if (attackCD >= 0)
        {
            attackCD -= Time.deltaTime;
        }
        else
        {
            Tower temp = GetTowerInSight();
            if (temp == null) enemyState = EnemyState.move;

            Attack(temp);
            attackCost = maxAttackCost;
            CaculateAttackCD();
        }

        if (attackCost > 0)
        {
            attackCost -= Time.deltaTime;
        }

        if (towerInBlock.Count > 0)
        {
            onBlock = true;
        }
        else
        {
            onBlock = false;
        }
    }

    #endregion

    #region fixUpdate

    protected override void FixAttack()
    {
        if (onBlock == true)
        {
            SetMainVelocity(1);
        }
        else
        {
            if (attackCost > 0)
            {
                SetMainVelocity(0);
            }
            else
            {
                SetMainVelocity(1);
            }
        }
    }


    #endregion



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.TryGetComponent<Tower>(out Tower tower))
            {
                if (!towerInBlock.Contains(tower))
                {
                    towerInBlock.Add(tower);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.TryGetComponent<Tower>(out Tower tower))
            {
                if (!towerInBlock.Contains(tower))
                {
                    towerInBlock.Remove(tower);
                }
            }
        }
    }

}

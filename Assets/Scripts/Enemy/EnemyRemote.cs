using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyRemote : Enemy
{

    protected float maxAttackCost = 0;//����ʱ��ͣ��
    protected float attackCost = 0;
    [SerializeField]
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
        if (targetPoint != null)
        {
            if (Vector2.Distance(gameObject.transform.position, targetPoint.GetPos() + offset) < minMoveOffset)
            {
                targetPoint = targetPoint.getNextPoint();
                if (targetPoint != null)
                {
                    if (!targetPoint.IsEndOfTheRoad()) RefreshOffsetToCenter();
                    else offset = Vector2.zero;//���յ��ƶ�ʱȡ��ƫ��
                }
            }
        }
        else
        {
            enemyState = EnemyState.idle;
        }


        if (attackCost > 0)
        {
            attackCost -= Time.deltaTime;
        }
        else
        {
            if (attackCD >= 0)
            {
                attackCD -= Time.deltaTime;
            }
            else
            {
                Tower temp = GetTowerInSight();
                if (temp == null)
                {
                    enemyState = EnemyState.move;
                    return;
                }

                Attack(temp);
                attackCost = maxAttackCost;
                CaculateAttackCD();
            }
        }

        if (towerInBlock.Count > 0)
        {
            Debug.Log("onblock");
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
            SetMainVelocity(0);
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
        if (collision.transform == transform)
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
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform == transform)
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

}

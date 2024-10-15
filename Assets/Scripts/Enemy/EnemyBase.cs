using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyData;

public class EnemyBase : MonoBehaviour
{
    //��ȡ���ݵ�����
    [SerializeField]
    protected string enemyName;

    public EnemyType enemyType;

    //�洢��ȡ����Ϣ��Ҳ������ÿ�γ�������ʱ���´�Data�л�ȡ
    protected EnemyInfo saving;

    //��ǰ��ֵ
    protected float currentHealth;
    protected float currentAttackDamage;
    protected float currentAttackSpeed;
    protected float currentAttackRange;
    protected float currentMoveSpeed;
    protected int currentPriority;

}

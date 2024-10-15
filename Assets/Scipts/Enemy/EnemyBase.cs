using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyData;

public class EnemyBase : MonoBehaviour
{
    //读取数据的依据
    [SerializeField]
    protected string enemyName;

    public EnemyType enemyType;

    //存储读取的信息，也可以在每次出入对象池时重新从Data中获取
    protected EnemyInfo saving;

    //当前数值
    protected float currentHealth;
    protected float currentAttackDamage;
    protected float currentAttackSpeed;
    protected float currentAttackRange;
    protected float currentMoveSpeed;
    protected int currentPriority;

}

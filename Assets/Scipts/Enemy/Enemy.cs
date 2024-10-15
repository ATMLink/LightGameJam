using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Enemy : EnemyBase
{


    protected Rigidbody2D rigid;
    protected new PolygonCollider2D collider;
    protected EnemySight sight1;

    #region generate
    private bool init = false;
    private Vector2 offset = Vector2.zero;
    [SerializeField]
    private float maxGenerateOffset = 100f;
    //ֵ�̶���0-100�ڣ��ֲ�����Ϊ���η��ȡ�
    //�����ϴ�����ֵ��С����Ϊ��Ҫʹ�価���ֲ��ڹֳ�����λ�ã����������һ�㡣

    #endregion

    #region enemyState

    enum EnemyState
    {
        idle,//����
        attack,
        move,
        skill,
        stun
    }
    EnemyState enemyState;

    private RoadPoint targetPoint;
    private float minMoveOffset = 0.2f;
    //ֻ�赽��Ŀ��㸽���Ϳ�������Ŀ���ƶ���
    private float stunCD = 0;

    #endregion

    #region enemyAttack

    private float attackCD = 0;

    #endregion



    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        //��������
        collider = GetComponent<PolygonCollider2D>();
        //��Ұ������
        sight1 = gameObject.transform.GetChild(0).GetComponent<EnemySight>();
        enemyState = EnemyState.move;
    }


    private void OnEnable()
    {
        if (init == false)
        {
            if (EnemyData.enemyDic.TryGetValue(enemyName, out saving))
            {
                init = true;
            }
            else
            {
                ReturnToPool();
                return;
            }
        }

        currentAttackDamage = saving.attack;
        currentHealth = saving.health;
        currentAttackRange = saving.attackRange;
        currentAttackSpeed = saving.attackSpeed;
        currentMoveSpeed = saving.moveSpeed;
        currentPriority = saving.priority;

        RefreshOffsetToCenter();    
    }


    private void OnDisable()
    {
        targetPoint = null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnHit(5);
        }
        EnemyAction();
    }

    private void FixedUpdate()
    {
        MoveController();
    }

    protected virtual void EnemyAction()
    {
        switch (enemyState)
        {
            case EnemyState.idle:
                if (targetPoint != null) enemyState = EnemyState.move;
                OnHit(5 * Time.deltaTime);
                break;
            case EnemyState.move:
                MovePlan();
                break;
            case EnemyState.attack:
                AttackPlan();
                break;
            case EnemyState.skill:
                SkillPlan();
                break;
            case EnemyState.stun:
                StunPlan(); 
                break;
        }
    }
    #region enemyStatePlan
    protected virtual void MovePlan()
    {
        if (targetPoint != null)
        {
            if(Vector2.Distance(gameObject.transform.position, targetPoint.GetPos() + offset) < minMoveOffset)
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
        if (sight1.towerInSight.Count > 0)
        {
            //�л�������״̬
            enemyState = EnemyState.attack;
            //���赲�󲻻����̹������������ѹ��
            CaculateAttackCD();
        }
    }
    protected virtual void AttackPlan()
    {
        if (attackCD >= 0)
        {
            attackCD -= Time.deltaTime;
        }
        else
        {
            //Tower temp;

            //����д���Ӷ��Ŀ����ѡȡ���ȹ���Ŀ��ĺ���

            Attack();
            CaculateAttackCD();
        }
    }

    protected virtual void SkillPlan()
    {

    }

    protected virtual void StunPlan()
    {
        //������/��ֱʱ���޷��������ƶ�������׼���ͷŵĹ����ͼ��ܾ��ᱻ���
        if (stunCD >= 0) stunCD -= Time.deltaTime;
        else enemyState = EnemyState.move;
    }


    protected virtual void Attack(/*Tower tower*/)
    {
        //����������ñ��������������˺���
    }

    private void CaculateAttackCD()
    {
        attackCD = 1 / currentAttackSpeed;
    }

    #endregion
    protected virtual void MoveController()
    {
        //��Ҫ�����ƶ�����ֹ����

        switch (enemyState)
        {
            case EnemyState.idle:
                rigid.velocity = Vector2.zero;
                break;
            case EnemyState.move:
                FixMove();
                break;
            case EnemyState.attack:
                rigid.velocity = Vector2.zero;
                break;
            case EnemyState.skill:
                rigid.velocity = Vector2.zero;
                break;
            case EnemyState.stun:
                rigid.velocity = Vector2.zero;
                break;
        }

    }

    #region EnemyMoveController

    private void FixMove()
    {
        if (targetPoint != null)
        {
            rigid.velocity = 18 * (targetPoint.GetPos() + offset - new Vector2(transform.position.x, transform.position.y)).normalized * currentMoveSpeed * Time.fixedDeltaTime;
        }
    }


    private void RefreshOffsetToCenter()
    {
        float offsetAngle = Random.Range(0, 360f);
        float radians = Mathf.Deg2Rad * offsetAngle;

        offset = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)).normalized;

        float offsetToCenter = EnemyData.GenerateStrategy(Random.Range(0, maxGenerateOffset));

        offset *= offsetToCenter;
    }

    #endregion




    protected virtual void Destroy()
    {
        //��������׶�

        ReturnToPool();
    }

    private void ReturnToPool()
    {
        EnemyEventSystem.instance.EnemyDestroy(this, enemyName);
    }

    public void SetMap(RoadPoint roadPoint)
    {
        targetPoint = roadPoint;
    }

    public float GetHealthRemain()
    {
        return currentHealth;
    }
    public float GetHealthPercent()
    {
        return currentHealth/saving.health;
    }

    //��ʱδ��չ
    public virtual void OnHit(float damage/*,�˺�����*/)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Destroy();
        }
    }

    public virtual void OnStun(float stunTime)
    {
        stunCD = stunTime;
        enemyState = EnemyState.stun;
    }

    public string GetEnemyName()
    {
        return enemyName; 
    }

    public int GetEnemyPriority()
    {
        return currentPriority;
    }
}

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
    //值固定在0-100内，分布策略为二次反比。
    //理论上大怪这个值更小，因为需要使其尽量分布在怪潮中心位置，看起来舒服一点。

    #endregion

    #region enemyState

    enum EnemyState
    {
        idle,//待机
        attack,
        move,
        skill,
        stun
    }
    EnemyState enemyState;

    private RoadPoint targetPoint;
    private float minMoveOffset = 0.2f;
    //只需到达目标点附近就可以向新目标移动了
    private float stunCD = 0;

    #endregion

    #region enemyAttack

    private float attackCD = 0;

    #endregion



    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        //自身触发器
        collider = GetComponent<PolygonCollider2D>();
        //视野触发器
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
                    else offset = Vector2.zero;//向终点移动时取消偏移
                }
            }
        }
        else
        {
            enemyState = EnemyState.idle;
        }
        if (sight1.towerInSight.Count > 0)
        {
            //切换到攻击状态
            enemyState = EnemyState.attack;
            //被阻挡后不会立刻攻击，减少玩家压力
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

            //这里写个从多个目标中选取优先攻击目标的函数

            Attack();
            CaculateAttackCD();
        }
    }

    protected virtual void SkillPlan()
    {

    }

    protected virtual void StunPlan()
    {
        //被击晕/僵直时，无法攻击和移动，所有准备释放的攻击和技能均会被打断
        if (stunCD >= 0) stunCD -= Time.deltaTime;
        else enemyState = EnemyState.move;
    }


    protected virtual void Attack(/*Tower tower*/)
    {
        //这个函数调用被攻击的塔的受伤函数
    }

    private void CaculateAttackCD()
    {
        attackCD = 1 / currentAttackSpeed;
    }

    #endregion
    protected virtual void MoveController()
    {
        //主要控制移动，防止卡顿

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
        //触发亡语阶段

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

    //暂时未拓展
    public virtual void OnHit(float damage/*,伤害类型*/)
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

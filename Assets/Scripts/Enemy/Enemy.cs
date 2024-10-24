using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Enemy : EnemyBase
{

    protected bool firstInit = true;

    protected Material material;
    private float flashDuration = 0.1f;

    protected Rigidbody2D rigid;
    protected new PolygonCollider2D collider;
    protected EnemySight sight1;    

    #region generate
    private bool init = false;
    protected Vector2 offset = Vector2.zero;
    [SerializeField]
    private float maxGenerateOffset = 100f;
    //ֵ�̶���0-100�ڣ��ֲ�����Ϊ���η��ȡ�
    //�����ϴ�����ֵ��С����Ϊ��Ҫʹ�価���ֲ��ڹֳ�����λ�ã����������һ�㡣

    private float minRotationSpeed = 25f; // ��С��ת�ٶ�
    private float maxRotationSpeed = 50f; // �����ת�ٶ�
    private float currentRotationSpeed = 0;

    #endregion

    #region enemyState

    protected enum EnemyState
    {
        idle,//����
        attack,
        move,
        skill,
        stun
    }
    protected EnemyState enemyState;

    protected RoadPoint targetPoint;
    protected float minMoveOffset = 0.2f;
    //ֻ�赽��Ŀ��㸽���Ϳ�������Ŀ���ƶ���
    private float stunCD = 0;

    #endregion

    #region enemyAttack

    protected float attackCD = 0;

    #endregion


    private float environmentSpeed = 1f;

    private float maxDarkSpeedUp = 8;
    private float DarkSpeedUp = 8;


    //���ƽ����Խ�
    private float landmarkSpeed = 1;



    protected virtual void Start()
    {
        //shader
        material = GetComponent<Renderer>().material;

        rigid = GetComponent<Rigidbody2D>();
        //��������
        collider = GetComponent<PolygonCollider2D>();
        //��Ұ������
        sight1 = gameObject.transform.GetChild(0).GetComponent<EnemySight>();
        enemyState = EnemyState.move;

        firstInit = false;
        EnableSet();
    }


    private void OnEnable()
    {
        EnableSet();
    }

    protected void EnableSet()
    {
        if (firstInit) return;
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

        ExtraEnableSet();
        CopyData();
        RefreshOffsetToCenter();
    }

    protected virtual void CopyData()
    {
        currentAttackDamage = saving.attack;
        currentHealth = saving.health;
        currentAttackRange = saving.attackRange;
        currentAttackSpeed = saving.attackSpeed;
        currentMoveSpeed = saving.moveSpeed;
        currentPriority = saving.priority;
    }

    protected virtual void ExtraEnableSet()
    {
        sight1.Clear();
        enemyState = EnemyState.move;
        // ��תЧ��
        float randomRotation = Random.Range(0f, 360f);
        transform.rotation = Quaternion.Euler(0, 0, randomRotation);
        currentRotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
        //����Ӱ��
        environmentSpeed = 1;
        maxDarkSpeedUp = EnemyData.maxDarkSpeedUp;
        DarkSpeedUp = maxDarkSpeedUp;
    }

    private void OnDisable()
    {
        targetPoint = null;
        ExtraDisableSet();
    }

    protected virtual void ExtraDisableSet()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnHit(1);
        }
        DarkTest();
        EnemyAction();
    }

    protected void DarkTest()
    {
        if(DarkSpeedUp == maxDarkSpeedUp)
        {
            if (LightSystem.Instance.IsIrradiated(transform.position))
            {
                DarkSpeedUp = 0;
            }
        }
    }


    private void FixedUpdate()
    {
        MoveController();
    }

    protected virtual void EnemyAction()
    {
        GlobleSkill();
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

    protected virtual void GlobleSkill()
    {

    }


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
            Tower temp = GetTowerInSight();
            if (temp == null)
            {
                enemyState = EnemyState.move;
                return;
            }
            else
            {
                Attack(temp);

                temp = GetTowerInSight();
                if (temp == null)
                {
                    enemyState = EnemyState.move;
                    return;
                }//����������Ұ����������ǰ��

                CaculateAttackCD();
            }
        }
    }

    protected virtual Tower GetTowerInSight()
    {
        sight1.Refresh();
        Tower temp = null;
        foreach(var tower in sight1.towerInSight)
        {
            if (tower == null) continue;
            if (temp == null) temp = tower;
            else
            {
                if (Vector3.Distance(tower.transform.position, transform.position) < Vector3.Distance(temp.transform.position, transform.position))
                {
                    temp = tower;
                }
            }
        }
        return temp;
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


    protected virtual void Attack(Tower tower)
    {
        tower.OnHit((int)currentAttackDamage);
    }

    protected void CaculateAttackCD()
    {
        attackCD = 1 / currentAttackSpeed;
    }

    #endregion
    protected virtual void MoveController()
    {
        //��Ҫ�����ƶ�����ֹ����


        rigid.angularVelocity = currentRotationSpeed;

        switch (enemyState)
        {
            case EnemyState.idle:
                SetMainVelocity(0);
                break;
            case EnemyState.move:
                FixMove();
                break;
            case EnemyState.attack:
                FixAttack();
                break;
            case EnemyState.skill:
                SetMainVelocity(0);
                break;
            case EnemyState.stun:
                SetMainVelocity(0);
                break;
        }

    }

    #region EnemyMoveController

    protected virtual void FixMove()
    {
        if (targetPoint != null)
        {
            SetMainVelocity(1);
        }
    }

    protected virtual void FixAttack()
    {
        SetMainVelocity(0);
    }

    protected void RefreshOffsetToCenter()
    {
        float offsetAngle = Random.Range(0, 360f);
        float radians = Mathf.Deg2Rad * offsetAngle;

        offset = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)).normalized;

        float offsetToCenter = EnemyData.GenerateStrategy(Random.Range(0, maxGenerateOffset));

        offset *= offsetToCenter;
    }




    protected virtual void SetMainVelocity(float v)
    {
        if (v == 0) rigid.velocity = Vector3.zero;
        else
        {
            if (DarkSpeedUp == 0)
            {
                rigid.velocity = v * currentMoveSpeed
                    * (targetPoint.GetPos() + offset - new Vector2(transform.position.x, transform.position.y)).normalized
                    * environmentSpeed * landmarkSpeed;
            }
            else
            {
                rigid.velocity = DarkSpeedUp
                    * (targetPoint.GetPos() + offset - new Vector2(transform.position.x, transform.position.y)).normalized
                    * environmentSpeed * landmarkSpeed;
            }
        }
    }

    #endregion


    protected virtual void Destroy()
    {
        //��������׶�

        ReturnToPool();
    }

    protected void ReturnToPool()
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
        StartCoroutine(OnHitShow());
    }

    public virtual void OnHeal(float heal)
    {
        currentHealth += heal;
        if (currentHealth > saving.health) currentHealth = saving.health;
    }

    protected virtual IEnumerator OnHitShow()
    {
        float elapsed = 0f;
        material.SetFloat("_FlashAmount", 1);
        while (elapsed < flashDuration)
        {
            elapsed += Time.deltaTime;
            material.SetFloat("_FlashAmount", Mathf.Lerp(1, 0, elapsed / flashDuration));
            yield return null;
        }
        material.SetFloat("_FlashAmount", 0);
        if (currentHealth <= 0)
        {
            Destroy();
        }
    }


    public virtual void SetSpeed(float environmentEffect)
    {
        environmentSpeed = environmentEffect;
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

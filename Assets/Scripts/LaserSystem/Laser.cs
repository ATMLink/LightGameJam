//using UnityEngine;

//public class Laser : MonoBehaviour
//{
//    public Tower sourceTower;
//    public float intensity; // 激光强度
//    public Vector3 direction; // 激光发射方向
//    public float damage; // 激光对敌人造成的伤害
//    public float maxDistance = 20f; // 激光最大距离
//    [SerializeField] private LineRenderer lineRenderer; // 用于可视化激光
//    [SerializeField] private ParticleSystem hitEffect; // 激光击中效果的粒子系统

//    private BoxCollider2D boxCollider2D;
//    private bool isActive = true; // 激光是否有效


//    public void Initialize(Tower source, Vector3 position, Vector3 direction, float intensity)
//    {
//        sourceTower = source; // 设置激光来源
//        transform.position = position;
//        this.direction = direction;
//        this.intensity = intensity;
//        boxCollider2D = GetComponent<BoxCollider2D>();
//    }
//    /*
//    public void Initialize()
//        {
//        lineRenderer = gameObject.AddComponent<LineRenderer>();
//        lineRenderer.startWidth = 0.1f;
//        lineRenderer.endWidth = 0.1f;
//        lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
//        lineRenderer.startColor = Color.red; // 根据强度设置颜色
//        lineRenderer.endColor = Color.red;
//        lineRenderer.positionCount = 2;

//        }
//    */
//    public void UpdateState()
//    {
//        if (isActive)
//        {
//            UpdateLaser();
//        }
//        else
//        {
//            // 如果激光失效，可以隐藏或清除激光
//            lineRenderer.enabled = false;
//        }
//    }

//    public void ResetLaser()
//    {
//        intensity = 0;
//        direction = Vector3.zero;
//        damage = 0;
//        SetLaserActive(false); // 隐藏激光
//    }

//    void UpdateLaser()
//    {
//        Vector3 endPoint = transform.position + direction.normalized * maxDistance; // 方向标准化
//        lineRenderer.SetPosition(0, transform.position);

//        // 更新激光终点，处理敌人遮挡情况
//        endPoint = GetAdjustedLaserEndPoint(endPoint);
//        lineRenderer.SetPosition(1, endPoint);
//        Vector3 relativePosition = endPoint - transform.position;
//        float lineLength = relativePosition.magnitude;
//        Transform endBeam = transform.GetChild(1);
//        Transform endParticle = hitEffect.transform;
//        endBeam.localPosition = new Vector3(lineLength, 0, 0);
//        endParticle.localPosition = new Vector3(lineLength, 0, 0);
//        float angle = (relativePosition.y > 0 ? 1 : -1) * Vector3.Angle(relativePosition.normalized, new Vector3(1, 0, 0));
//        transform.localRotation = Quaternion.Euler(0, 0, angle);
//        //collider
//        boxCollider2D = GetComponent<BoxCollider2D>();
//        boxCollider2D.offset = new Vector2(lineLength / 2, 0);
//        boxCollider2D.size = new Vector2(lineLength, boxCollider2D.size.y);
//        Attack();
//    }

//    /// <summary>
//    /// 更新激光的起点和方向
//    /// </summary>
//    public void UpdateLaserPositionAndDirection(Vector3 newPosition, Vector3 newDirection)
//    {
//        transform.position = newPosition;
//        direction = newDirection;
//        UpdateLaser();
//    }

//    private Vector3 GetAdjustedLaserEndPoint(Vector3 intendedEndPoint)
//    {
//        // 激光的发射方向和长度
//        Vector3 adjustedOrigin = transform.position + (Vector3)(direction.normalized * 0.1f); // 稍微偏移射线的起点
//        Vector2 laserOrigin = new Vector2(adjustedOrigin.x, adjustedOrigin.y); // 转换为 Vector2 用于射线
//        Vector2 laserDirection = direction.normalized;
//        float laserDistance = maxDistance;

//        // Debug.Log($"Adjusted Laser Origin: {laserOrigin}, Direction: {laserDirection}, Distance: {laserDistance}");

//        // 忽略 Laser, TileMap 和 Tower 层的 layerMask
//        int layerMask = ~(
//            LayerMask.GetMask("Laser") |
//            LayerMask.GetMask("TileMap") |
//            LayerMask.GetMask("Tower") |
//            LayerMask.GetMask("TowerSight") |
//            LayerMask.GetMask("EnemySight") |
//            LayerMask.GetMask("tile") |
//            LayerMask.GetMask("Default")
//        );




//        hitEffect.gameObject.SetActive(false);
//        Debug.Log("!!!");
//        RaycastHit2D hit = Physics2D.Raycast(laserOrigin, laserDirection, laserDistance, layerMask);

//        if (hit.collider != null)
//        {
//            Debug.Log($"Hit detected at: {hit.point} with collider: {hit.collider.name}");

//            // 根据碰撞对象的 Layer 进行检查
//            int enemyLayer = LayerMask.NameToLayer("Enemy");
//            int towerLayer = LayerMask.NameToLayer("TowerWall");
//            int receiverLayer = LayerMask.NameToLayer("LaseerReceiver");

//            if (hit.collider.gameObject.layer == enemyLayer)
//            {
//                return AdjustEndPoint(hit, laserDirection);
//            }
//            else if (hit.collider.gameObject.layer == towerLayer)
//            {
//                var wallComponent = hit.collider.GetComponent<Wall>();
//                if (wallComponent == null || !wallComponent.canLightThrough)
//                {
//                    hitEffect.gameObject.SetActive(true);

//                    return hit.point;
//                }
//            }
//            else if (hit.collider.gameObject.layer == receiverLayer)
//            {
//                hitEffect.gameObject.SetActive(false);
//                return AdjustEndPoint(hit, laserDirection);
//            }
//            else if (hit.collider.GetComponent<TilemapFeature>()?.canLightThrough == true)
//            {
//                hitEffect.gameObject.SetActive(true);

//                return AdjustEndPoint(hit, laserDirection);
//            }
//            else
//            {
//                return hit.point; // 返回碰撞点作为激光的终点
//            }
//        }

//        // Debug.Log("No hit detected, laser reached intended endpoint.");
//        return intendedEndPoint;
//    }

//    private Vector3 AdjustEndPoint(RaycastHit2D hit, Vector2 laserDirection)
//    {
//        Vector3 hitPoint = hit.point;
//        float offsetDistance = -0.1f;

//        // Debug.Log($"Adjusting endpoint from {hitPoint} with offset {offsetDistance} in direction {laserDirection}");

//        return hitPoint - (Vector3)laserDirection * offsetDistance;
//    }

//    private void Attack()
//    {
//        RaycastHit hit;
//        if (Physics.Raycast(transform.position, direction, out hit, maxDistance))
//        {
//            // 检查激光是否击中敌人
//            if (hit.collider.CompareTag("Enemy"))
//            {
//                // 造成伤害
//                hit.collider.GetComponent<Enemy>().OnHit(damage);
//                // TriggerHitEffect(hit.point); // 触发击中效果
//            }
//        }
//    }

//    private void TriggerHitEffect(Vector3 position)
//    {
//        // 在激光击中位置播放粒子效果
//        if (hitEffect != null)
//        {
//            hitEffect.gameObject.SetActive(true);
//            //ParticleSystem effect = Instantiate(hitEffect, position, Quaternion.identity);
//            hitEffect.Play();
//            //Destroy(effect.gameObject, effect.main.duration); // 播放完成后销毁粒子效果
//        }
//    }

//    public void SetLaserProperties(float newIntensity, Vector3 newDirection)
//    {
//        intensity = newIntensity;
//        direction = newDirection;
//        UpdateLaser();
//    }

//    // 控制激光的有效性
//    public void SetLaserActive(bool active)
//    {
//        isActive = active;
//        lineRenderer.enabled = active; // 根据激光状态来显示或隐藏激光
//        hitEffect.gameObject.SetActive(active);
//        transform.GetChild(1).gameObject.SetActive(active);
//        Debug.Log($"Laser {gameObject.name} active: {active}"); // 输出激光的激活状态
//    }


//}


using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public Tower sourceTower;
    public float intensity; // 激光强度
    public Vector3 direction; // 激光发射方向
    public float damage; // 激光对敌人造成的伤害
    public float maxDistance = 20f; // 激光最大距离
    [SerializeField] private LineRenderer lineRenderer; // 用于可视化激光
    [SerializeField] private ParticleSystem hitEffect; // 激光击中效果的粒子系统

    public List<Enemy> enemies;
    
    private BoxCollider2D boxCollider2D;
    private bool isActive = true; // 激光是否有效

    private List<GameObject> hitObj = new List<GameObject>();
    [SerializeField]
    private TextMeshProUGUI textMeshProUGUI;


    public void Initialize(Tower source, Vector3 position, Vector3 direction, float intensity)
    {
        sourceTower = source; // 设置激光来源 
        transform.position = new Vector3(position.x, position.y, 0);
        this.direction = direction;
        this.intensity = intensity;
        lineRenderer.material.SetFloat("_Intensity", this.intensity);
        boxCollider2D = GetComponent<BoxCollider2D>();
        textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
        hitObj.Clear();
    }
    /*
    public void Initialize()
        {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
        lineRenderer.startColor = Color.red; // 根据强度设置颜色
        lineRenderer.endColor = Color.red;
        lineRenderer.positionCount = 2;

        }
    */
    public void UpdateState()
    {
        if (isActive)
        {
            textMeshProUGUI.text =(int)intensity + "";
            UpdateLaser();
        }
        else
        {
            // 如果激光失效，可以隐藏或清除激光
            lineRenderer.enabled = false;
        }
    }

    public void ResetLaser()
    {

        foreach (GameObject obj in hitObj)
        {
            Tower tower = obj.GetComponent<Tower>();
            if (tower != null)
            {
                tower.OnLaserOut(this);
            }
            if (obj.CompareTag("Enemy"))
            {
                //
            }
        }
        hitObj.Clear();

        intensity = 0;
        direction = Vector3.zero;
        damage = 0;
        //SetLaserActive(false); // 隐藏激光
        
    }

    void UpdateLaser()
    {
        Vector3 endPoint = transform.position + direction.normalized * maxDistance; // 方向标准化
        lineRenderer.SetPosition(0, transform.position);

        // 更新激光终点，处理敌人遮挡情况
        endPoint = GetAdjustedLaserEndPoint(endPoint);
        lineRenderer.SetPosition(1, endPoint);
        Vector3 relativePosition = endPoint - transform.position;
        float lineLength = relativePosition.magnitude;
        Transform endBeam = transform.GetChild(1);
        Transform endParticle = hitEffect.transform;
        endBeam.localPosition = new Vector3(lineLength, 0, 0);
        endParticle.localPosition = new Vector3(lineLength, 0, 0);
        float angle = (relativePosition.y > 0 ? 1 : -1) * Vector3.Angle(relativePosition.normalized, new Vector3(1, 0, 0));
        transform.localRotation = Quaternion.Euler(0, 0, angle);
        //collider
        boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.offset = new Vector2(lineLength / 2, 0);
        boxCollider2D.size = new Vector2(lineLength, boxCollider2D.size.y);
        Attack();
    }

    /// <summary>
    /// 更新激光的起点和方向
    /// </summary>
    public void UpdateLaserPositionAndDirection(Vector3 newPosition, Vector3 newDirection)
    {
        transform.position = newPosition;
        direction = newDirection;
        UpdateLaser();
    }

    private Vector3 GetAdjustedLaserEndPoint(Vector3 intendedEndPoint)
    {
        // 激光的发射方向和长度
        Vector3 adjustedOrigin = transform.position + (Vector3)(direction.normalized * 0.1f); // 稍微偏移射线的起点
        Vector2 laserOrigin = new Vector2(adjustedOrigin.x, adjustedOrigin.y); // 转换为 Vector2 用于射线
        Vector2 laserDirection = direction.normalized;
        float laserDistance = maxDistance;

        // Debug.Log($"Adjusted Laser Origin: {laserOrigin}, Direction: {laserDirection}, Distance: {laserDistance}");

        // 忽略 Laser, TileMap 和 Tower 层的 layerMask
        int layerMask = ~(
            LayerMask.GetMask("Laser") |
            LayerMask.GetMask("TileMap") |
            LayerMask.GetMask("Tower") |
            LayerMask.GetMask("TowerSight") |
            LayerMask.GetMask("EnemySight") |
            LayerMask.GetMask("tile") |
            LayerMask.GetMask("Default")
        );




        hitEffect.gameObject.SetActive(false);
        //Debug.Log("!!!");
        RaycastHit2D hit = Physics2D.Raycast(laserOrigin, laserDirection, laserDistance, layerMask);

        if (hit.collider != null)
        {
            //Debug.Log($"Hit detected at: {hit.point} with collider: {hit.collider.name}");

            // 根据碰撞对象的 Layer 进行检查
            int enemyLayer = LayerMask.NameToLayer("Enemy");
            int towerLayer = LayerMask.NameToLayer("TowerWall");
            int receiverLayer = LayerMask.NameToLayer("LaseerReceiver");

            if (hit.collider.gameObject.layer == enemyLayer)
            {
                return AdjustEndPoint(hit, laserDirection);
            }
            else if (hit.collider.gameObject.layer == towerLayer)
            {
                var wallComponent = hit.collider.GetComponent<Wall>();
                if (wallComponent == null || !wallComponent.canLightThrough)
                {
                    hitEffect.gameObject.SetActive(true);

                    return hit.point;
                }
            }
            else if (hit.collider.gameObject.layer == receiverLayer)
            {
                hitEffect.gameObject.SetActive(false);
                return AdjustEndPoint(hit, laserDirection);
            }
            else if (hit.collider.GetComponent<TilemapFeature>()?.canLightThrough == true)
            {
                hitEffect.gameObject.SetActive(true);

                return AdjustEndPoint(hit, laserDirection);
            }
            else
            {
                return hit.point; // 返回碰撞点作为激光的终点
            }
        }

        // Debug.Log("No hit detected, laser reached intended endpoint.");
        return intendedEndPoint;
    }

    private Vector3 AdjustEndPoint(RaycastHit2D hit, Vector2 laserDirection)
    {
        Vector3 hitPoint = hit.point;
        float offsetDistance = -0.1f;

        // Debug.Log($"Adjusting endpoint from {hitPoint} with offset {offsetDistance} in direction {laserDirection}");

        return hitPoint - (Vector3)laserDirection * offsetDistance;
    }

    private void Attack()
    {
        if (enemies != null)
        {
            List<Enemy> list = new List<Enemy>(enemies);
            foreach (var enemy in list)
            {
                if (enemy.gameObject.activeInHierarchy)
                {
                    enemy.OnHit(0.001f * intensity);
                }
                // enemy.OnHit(1000f);
            }
        }
                // 造成伤害
                // hit.collider.GetComponent<Enemy>().OnHit(damage);
    }

    private void TriggerHitEffect(Vector3 position)
    {
        // 在激光击中位置播放粒子效果
        if (hitEffect != null)
        {
            hitEffect.gameObject.SetActive(true);
            //ParticleSystem effect = Instantiate(hitEffect, position, Quaternion.identity);
            hitEffect.Play();
            //Destroy(effect.gameObject, effect.main.duration); // 播放完成后销毁粒子效果
        }
    }

    public void SetLaserProperties(float newIntensity, Vector3 newDirection)
    {
        SetLaserIntensity(newIntensity);
        direction = newDirection;
        UpdateLaser();
    }

    public void SetLaserIntensity(float newIntensity)
    {
        intensity = newIntensity;
        lineRenderer.material.SetFloat("_Intensity", intensity);
        UpdateLaser();
    }

    // 控制激光的有效性
    public void SetLaserActive(bool active)
    {
        isActive = active;
        lineRenderer.enabled = active; // 根据激光状态来显示或隐藏激光
        hitEffect.gameObject.SetActive(active);
        transform.GetChild(1).gameObject.SetActive(active);
        //Debug.Log($"Laser {gameObject.name} active: {active}"); // 输出激光的激活状态
    }


    // protected void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (collision != null)
    //     {
    //         if (collision.CompareTag("Enemy"))
    //         {
    //             enemies.Add(collision.gameObject.GetComponent<Enemy>());
    //         }
    //     }
    // }

    protected void OnTriggerStay2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (!hitObj.Contains(collision.gameObject))
            {
                if (collision.CompareTag("Tower"))
                {
                    Tower tower = collision.GetComponent<Tower>();
                    if (tower != null)
                    {
                        Debug.Log("enter");
                        tower.OnLaserHit(this);
                    }
                    UpdateLaser();
                    hitObj.Add(collision.gameObject);
                }
                if (collision.CompareTag("Enemy"))
                {
                    Enemy enemy = collision.GetComponent<Enemy>();
                    enemies.Add(enemy);
                    UpdateLaser();
                    hitObj.Add(collision.gameObject);
                }
            }
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (hitObj.Contains(collision.gameObject))
            {
                if (collision.CompareTag("Tower"))
                {
                    Debug.Log("left");
                    Tower tower = collision.GetComponent<Tower>();
                    if (tower != null)
                    {
                        tower.OnLaserOut(this);
                    }
                }
                if (collision.CompareTag("Enemy"))
                {
                    enemies.Remove(collision.gameObject.GetComponent<Enemy>());
                }
                UpdateLaser();
                hitObj.Remove(collision.gameObject);
            }
        }
    }




}







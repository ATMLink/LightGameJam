using UnityEngine;

public class Laser : MonoBehaviour
{
    public float intensity; // 激光强度
    public Vector3 direction; // 激光发射方向
    public float damage; // 激光对敌人造成的伤害
    public float maxDistance = 20f; // 激光最大距离
    [SerializeField] private LineRenderer lineRenderer; // 用于可视化激光
    [SerializeField] private ParticleSystem hitEffect; // 激光击中效果的粒子系统

    private bool isActive = true; // 激光是否有效

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

    public void UpdateState()
    {
        if (isActive)
        {
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
        intensity = 0;
        direction = Vector3.zero;
        damage = 0;
        SetLaserActive(false); // 隐藏激光
    }

    void UpdateLaser()
    {
        Vector3 endPoint = transform.position + direction.normalized * maxDistance; // 方向标准化
        lineRenderer.SetPosition(0, transform.position);

        // 更新激光终点，处理敌人遮挡情况
        endPoint = GetAdjustedLaserEndPoint(endPoint);
        lineRenderer.SetPosition(1, endPoint);

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
        RaycastHit hit;
        // 检查激光是否击中敌人
        if (Physics.Raycast(transform.position, direction, out hit, maxDistance))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                // 计算敌人前面的位置
                Vector3 hitPoint = hit.point; // 激光击中的位置
                Vector3 laserOrigin = transform.position; // 激光起始位置
                Vector3 laserDirection = direction.normalized; // 激光方向

                // 计算敌人前方的点（按标准化方向缩短激光长度）
                float distanceToEnemy = Vector3.Distance(laserOrigin, hitPoint);
                float offsetDistance = 0.1f; // 确保激光不与敌人重叠

                // 如果距离小于offsetDistance，返回起始点
                if (distanceToEnemy <= offsetDistance)
                {
                    return laserOrigin; // 返回起始点，激光完全缩短
                }
                
                // 返回标准化方向计算的敌人前方位置
                return hitPoint - laserDirection * offsetDistance;
            }
        }

        return intendedEndPoint; // 如果没有击中敌人，返回原定终点
    }
    private void Attack()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, maxDistance))
        {
            // 检查激光是否击中敌人
            if (hit.collider.CompareTag("Enemy"))
            {
                // 造成伤害
                hit.collider.GetComponent<Enemy>().OnHit(damage);
                TriggerHitEffect(hit.point); // 触发击中效果
            }
        }
    }

    private void TriggerHitEffect(Vector3 position)
    {
        // 在激光击中位置播放粒子效果
        if (hitEffect != null)
        {
            ParticleSystem effect = Instantiate(hitEffect, position, Quaternion.identity);
            effect.Play();
            Destroy(effect.gameObject, effect.main.duration); // 播放完成后销毁粒子效果
        }
    }

    public void SetLaserProperties(float newIntensity, Vector3 newDirection)
    {
        intensity = newIntensity;
        direction = newDirection;
        UpdateLaser();
    }

    // 控制激光的有效性
    public void SetLaserActive(bool active)
    {
        isActive = active;
        lineRenderer.enabled = active; // 根据激光状态来显示或隐藏激光
        Debug.Log($"Laser {gameObject.name} active: {active}"); // 输出激光的激活状态
    }
}
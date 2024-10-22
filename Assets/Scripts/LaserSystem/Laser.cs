using UnityEngine;

public class Laser : MonoBehaviour
{
    public float intensity; // ����ǿ��
    public Vector3 direction; // ���ⷢ�䷽��
    public float damage; // ����Ե�����ɵ��˺�
    public float maxDistance = 20f; // ����������
    [SerializeField] private LineRenderer lineRenderer; // ���ڿ��ӻ�����
    [SerializeField] private ParticleSystem hitEffect; // �������Ч��������ϵͳ

    private bool isActive = true; // �����Ƿ���Ч

    public void Initialize()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
        lineRenderer.startColor = Color.red; // ����ǿ��������ɫ
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
            // �������ʧЧ���������ػ��������
            lineRenderer.enabled = false;
        }
    }

    public void ResetLaser()
    {
        intensity = 0;
        direction = Vector3.zero;
        damage = 0;
        SetLaserActive(false); // ���ؼ���
    }

    void UpdateLaser()
    {
        Vector3 endPoint = transform.position + direction.normalized * maxDistance; // �����׼��
        lineRenderer.SetPosition(0, transform.position);

        // ���¼����յ㣬��������ڵ����
        endPoint = GetAdjustedLaserEndPoint(endPoint);
        lineRenderer.SetPosition(1, endPoint);

        Attack();
    }
    
    /// <summary>
    /// ���¼�������ͷ���
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
        // ��鼤���Ƿ���е���
        if (Physics.Raycast(transform.position, direction, out hit, maxDistance))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                // �������ǰ���λ��
                Vector3 hitPoint = hit.point; // ������е�λ��
                Vector3 laserOrigin = transform.position; // ������ʼλ��
                Vector3 laserDirection = direction.normalized; // ���ⷽ��

                // �������ǰ���ĵ㣨����׼���������̼��ⳤ�ȣ�
                float distanceToEnemy = Vector3.Distance(laserOrigin, hitPoint);
                float offsetDistance = 0.1f; // ȷ�����ⲻ������ص�

                // �������С��offsetDistance��������ʼ��
                if (distanceToEnemy <= offsetDistance)
                {
                    return laserOrigin; // ������ʼ�㣬������ȫ����
                }
                
                // ���ر�׼���������ĵ���ǰ��λ��
                return hitPoint - laserDirection * offsetDistance;
            }
        }

        return intendedEndPoint; // ���û�л��е��ˣ�����ԭ���յ�
    }
    private void Attack()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, maxDistance))
        {
            // ��鼤���Ƿ���е���
            if (hit.collider.CompareTag("Enemy"))
            {
                // ����˺�
                hit.collider.GetComponent<Enemy>().OnHit(damage);
                TriggerHitEffect(hit.point); // ��������Ч��
            }
        }
    }

    private void TriggerHitEffect(Vector3 position)
    {
        // �ڼ������λ�ò�������Ч��
        if (hitEffect != null)
        {
            ParticleSystem effect = Instantiate(hitEffect, position, Quaternion.identity);
            effect.Play();
            Destroy(effect.gameObject, effect.main.duration); // ������ɺ���������Ч��
        }
    }

    public void SetLaserProperties(float newIntensity, Vector3 newDirection)
    {
        intensity = newIntensity;
        direction = newDirection;
        UpdateLaser();
    }

    // ���Ƽ������Ч��
    public void SetLaserActive(bool active)
    {
        isActive = active;
        lineRenderer.enabled = active; // ���ݼ���״̬����ʾ�����ؼ���
        Debug.Log($"Laser {gameObject.name} active: {active}"); // �������ļ���״̬
    }
}
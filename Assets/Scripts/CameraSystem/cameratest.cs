using UnityEngine;

public class CameraLeftBorder : MonoBehaviour
{
    void Update()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            // ���ӿ���߽��(0, 0.5, 0)ת��Ϊ��������
            Vector3 viewportPoint = new Vector3(0, 0, 0);
            Vector3 worldPoint = mainCamera.ViewportToWorldPoint(viewportPoint);
            float leftBorderX = worldPoint.x;
            Debug.Log("�������Ļ��߽������x����ֵ: " + worldPoint);
        }
    }
}

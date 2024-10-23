using UnityEngine;

public class CameraLeftBorder : MonoBehaviour
{
    void Update()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            // 将视口左边界点(0, 0.5, 0)转换为世界坐标
            Vector3 viewportPoint = new Vector3(0, 0, 0);
            Vector3 worldPoint = mainCamera.ViewportToWorldPoint(viewportPoint);
            float leftBorderX = worldPoint.x;
            Debug.Log("摄像机屏幕左边界的世界x坐标值: " + worldPoint);
        }
    }
}

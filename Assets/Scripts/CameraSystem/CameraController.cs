using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private FloatVariable moveSpeed;
    private FloatVariable zoomSpeed;
    private FloatVariable minZoom;
    private FloatVariable maxZoom;
    private Vector2Variable minBounds;
    private Vector2Variable maxBounds;
    private Camera camera;
    private Vector3 initialPosition;
    
    
    public void Initialize()
    {
        throw new System.NotImplementedException();
    }

    public void HandleCameraMovement()
    {
        throw new System.NotImplementedException();
    }
}

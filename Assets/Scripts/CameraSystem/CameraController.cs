using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private FloatVariable moveSpeed;
    [SerializeField] private FloatVariable zoomSpeed;
    [SerializeField] private FloatVariable minZoom;
    [SerializeField] private FloatVariable maxZoom;
    [SerializeField] private Vector2Variable minBounds;
    [SerializeField] private Vector2Variable maxBounds;
    private Camera camera;
    private Vector3 initialPosition;

    public void Initialize()
    {
        
        
        initialPosition = camera.transform.position;
    }

    public void UpdateState()
    {
        HandleCameraMovement();
        HandleCameraZoom();
    }

    public void HandleCameraMovement()
    {
        // Logic for dragging the camera with the left mouse button
        if (Input.GetMouseButton(0))
        {
            Vector3 delta = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0);
            camera.transform.Translate(-delta * moveSpeed.Value, Space.World);
            ClampCameraPosition();
        }
    }

    public void HandleCameraZoom()
    {
        // Logic for zooming in and out with the mouse scroll wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        float newZoom = Mathf.Clamp(camera.orthographicSize - scroll * zoomSpeed.Value, minZoom.Value, maxZoom.Value);
        camera.orthographicSize = newZoom;
    }

    public void ResetCameraPosition()
    {
        // Reset the camera to its initial position on middle mouse button click
        if (Input.GetMouseButton(2))
        {
            camera.transform.position = initialPosition;
        }
    }

    public void ClampCameraPosition()
    {
        // Ensure the camera doesn't move outside the boundaries
        Vector3 pos = camera.transform.position;
        pos.x = Mathf.Clamp(pos.x, minBounds.Value.x, maxBounds.Value.x);
        pos.y = Mathf.Clamp(pos.y, minBounds.Value.y, maxBounds.Value.y);
        camera.transform.position = pos;
    }
}

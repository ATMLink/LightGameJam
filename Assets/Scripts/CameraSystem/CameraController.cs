using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class CameraController : MonoBehaviour
{
    [SerializeField] private FloatVariable moveSpeed;
    [SerializeField] private FloatVariable zoomSpeed;
    [SerializeField] private FloatVariable minZoom;
    [SerializeField] private FloatVariable maxZoom;
    [SerializeField] private Vector2Variable minBounds;
    [SerializeField] private Vector2Variable maxBounds;
    [SerializeField] private TilemapManager tilemapManager;
    [SerializeField] private GameObject FunctionMenu;
    public bool isMoving;
    private Camera camera;  
    private Vector3 initialPosition;
    private Vector3 lastMouseWorldPosition;

    public void Initialize()
    {
        isMoving = false;
        // Initialize the camera component if it's not already assigned
        camera = Camera.main; // or GetComponent<Camera>() if the camera is attached to this gameObject
        if (camera == null)
        {
            Debug.LogError("Camera not found! Ensure there is a camera in the scene.");
            return;
        }

        // Set initial data
        // moveSpeed.SetValue(5f);
        // zoomSpeed.SetValue(2f);
        // minZoom.SetValue(5f);
        // maxZoom.SetValue(20f);
        // minBounds.SetValue(new (-10f, -10f));
        // maxBounds.SetValue(new (10f, 10f));
        initialPosition = new(0,0, -1);

        camera.transform.position = initialPosition;

        // Optional: Print out initial values for debugging
        Debug.Log($"Initial moveSpeed: {moveSpeed.Value}, zoomSpeed: {zoomSpeed.Value}, minZoom: {minZoom.Value}," +
                  $" maxZoom: {maxZoom.Value}");
    }

    public void UpdateState()
    {
        HandleCameraMovement();
        HandleCameraZoom();
        ResetCameraPosition();
        ChangeBounds();
        HideMenu();
    }

    public void HandleCameraMovement()// speed should not be absoluted
    {
        // Logic for dragging the camera with the right mouse button
        if (Input.GetMouseButton(1))
        {
            isMoving = true;
            Vector3 delta = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0);
            camera.transform.Translate(-delta * moveSpeed.Value, Space.World);
            ClampCameraPosition();
        }
        else isMoving = false;
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
        if (Input.GetButtonDown("Fire3"))
        {
            camera.transform.position = initialPosition;
            Debug.Log("Camera position reset to initial position.");
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
    public void ChangeBounds()
    {
        Vector3 viewportPoint = new Vector3(0, 0, 0);
        Vector3 worldPoint = camera.ViewportToWorldPoint(viewportPoint);
        float BorderX = camera.transform.position.x - worldPoint.x;
        float BorderY = camera.transform.position.y - worldPoint.y;
        minBounds.SetValue(new Vector2(-(tilemapManager.width / 2 - BorderX), -(tilemapManager.height / 2 - BorderY)));
        maxBounds.SetValue(new Vector2((tilemapManager.width / 2 - BorderX), tilemapManager.height / 2 - BorderY));
        ClampCameraPosition();
    }
    public void HideMenu() { 
        if (isMoving)FunctionMenu.gameObject.SetActive(false);
    
    
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public CameraController cameraController;
    public ConstructManager constructManager;

    void Update()
    {
        CaptureInput();
        HandleInput();
    }

    void CaptureInput()
    {
        // Logic for capturing user input (e.g., mouse clicks)
        cameraController.UpdateState();
    }

    void HandleInput()
    {
        // Right-click to interact with the map and bring up the construction menu
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // Tile tile = hit.collider.GetComponent<Tile>();
                // if (tile != null && tile.canBuild)
                // {
                //     constructManager.PlaceTower(hit.point);
                // }
            }
        }
    }
}

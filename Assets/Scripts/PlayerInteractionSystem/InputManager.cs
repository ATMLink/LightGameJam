using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InputManager : MonoBehaviour
{
    public CameraController cameraController;
    public ConstructManager constructManager;

    public void UpdateState()
    {
        CaptureInput();
        HandleInput();
    }

    public void CaptureInput()
    {
        // Logic for capturing user input (e.g., mouse clicks)
        cameraController.UpdateState();
    }

    public void HandleInput()
    {
        // Right-click to interact with the map and bring up the construction menu
        if (Input.GetMouseButtonDown(1))
        {
            // 2D Raycast from camera to the mouse position
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mouseWorldPos2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos2D, Vector2.zero);

            if (hit.collider != null)
            {
                // Assuming your tiles are using TilemapCollider2D and have some associated behavior
                Tilemap tilemap = hit.collider.GetComponent<Tilemap>();
                if (tilemap != null)
                {
                    // Get the clicked tile's position and round to nearest grid center
                    Vector3Int cellPosition = tilemap.WorldToCell(mouseWorldPos);
                    Vector3 cellCenterPos = tilemap.GetCellCenterWorld(cellPosition);

                    // Pass the cell center position instead of the exact hit point
                    constructManager.PlaceTower(cellCenterPos);
                }
            }
        }
    }
}

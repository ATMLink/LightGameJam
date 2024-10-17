using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InputManager : MonoBehaviour
{
    [SerializeField] private CameraController cameraController;
    [SerializeField] private ConstructManager constructManager;

    private bool isDraggingTower = false; // 是否正在拖拽塔
    private TowerAttributes currentDraggedTower; // 当前拖拽的塔属性
    public void UpdateState()
    {
        CaptureInput();
        HandleInput();
    }

    public void CaptureInput()
    {
        // Logic for capturing user input 
        cameraController.UpdateState();
    }

    public void HandleInput()
    {
        // 处理左键点击以放置塔
        if (isDraggingTower && Input.GetMouseButtonUp(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mouseWorldPos2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos2D, Vector2.zero);

            if (hit.collider != null)
            {
                Tilemap tilemap = hit.collider.GetComponent<Tilemap>();
                if (tilemap != null)
                {
                    Vector3Int cellPosition = tilemap.WorldToCell(mouseWorldPos);
                    Vector3 cellCenterPos = tilemap.GetCellCenterWorld(cellPosition);

                    // 放置塔在地图上
                    constructManager.PlaceTower(cellCenterPos);

                    // 放置完成，取消拖拽状态
                    isDraggingTower = false;
                    currentDraggedTower = null;
                }
            }
        }
    }
    
    public void StartDraggingTower(TowerAttributes towerAttributes)
    {
        isDraggingTower = true;
        currentDraggedTower = towerAttributes;
        constructManager.SelectTower(towerAttributes);
    }
    
}
// public void HandleInput()
// {
//     // Right-click to interact with the map and bring up the construction menu
//     if (Input.GetMouseButtonDown(1))
//     {
//         // 2D Raycast from camera to the mouse position
//         Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//         Vector2 mouseWorldPos2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);
//     
//         RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos2D, Vector2.zero);
//     
//         if (hit.collider != null)
//         {
//             // Assuming your tiles are using TilemapCollider2D and have some associated behavior
//             Tilemap tilemap = hit.collider.GetComponent<Tilemap>();
//             if (tilemap != null)
//             {
//                 // Get the clicked tile's position and round to nearest grid center
//                 Vector3Int cellPosition = tilemap.WorldToCell(mouseWorldPos);
//                 Vector3 cellCenterPos = tilemap.GetCellCenterWorld(cellPosition);
//     
//                 // Pass the cell center position instead of the exact hit point
//                 constructManager.PlaceTower(cellCenterPos);
//             }
//         }
//     }
// }
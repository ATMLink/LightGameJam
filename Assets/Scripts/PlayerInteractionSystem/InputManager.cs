using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InputManager : MonoBehaviour
{
    [SerializeField] private CameraController cameraController;
    [SerializeField] private ConstructManager constructManager;

    private bool isDraggingTower = false;
    private TowerAttributes currentDraggedTower;
    private GameObject towerPreview;

    public void UpdateState()
    {
        CaptureInput();
        HandleInput();
    }

    public void CaptureInput()
    {
        cameraController.UpdateState();
    }

    public void HandleInput()
    {
        if (isDraggingTower)
        {
            Vector3? position = GetPositionFromInput();
            if (position.HasValue)
            {
                // 更新预览塔的位置
                if (towerPreview != null)
                {
                    towerPreview.transform.position = position.Value;
                }

                // 放置塔的逻辑
                if (Input.GetMouseButtonUp(0))
                {
                    constructManager.PlaceTower(position.Value);
                    isDraggingTower = false;
                    currentDraggedTower = null;
                    Destroy(towerPreview);
                }
            }
        }

        // 右键点击弹出建筑菜单（可选的功能）
        if (Input.GetMouseButtonDown(1))
        {
            Vector3? position = GetPositionFromInput();
            if (position.HasValue)
            {
                constructManager.ShowConstructionMenu(position.Value);
            }
        }
    }

    public void StartDraggingTower(TowerAttributes towerAttributes)
    {
        isDraggingTower = true;
        currentDraggedTower = towerAttributes;
        constructManager.SelectTower(towerAttributes);

        // 创建预览塔
        towerPreview = new GameObject("TowerPreview");
        SpriteRenderer renderer = towerPreview.AddComponent<SpriteRenderer>();
        renderer.sprite = towerAttributes.towerSprite; // 设置预览塔的图片
        renderer.color = new Color(1, 1, 1, 0.5f); // 半透明显示
    }
    
    public Vector3 GetPositionFromInput()
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
                return cellCenterPos;
            }
        }

        return new Vector3(0,0,-1); // 点击的区域无效
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
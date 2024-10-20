using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InputManager : MonoBehaviour
{
    [SerializeField] private CameraController cameraController;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private ConstructManager constructManager;
    [SerializeField] private TowerManager towerManager;
    [SerializeField] private UiManager uiManager;

    public LayerMask tilemapLayerMask;
    
    private bool isDraggingTower = false;
    private TowerAttributes selectedTowerAttributes;
    private GameObject towerPreview;

    public void UpdateState()
    {
        //Debug.Log("InputManager UpdateState called.");
        CaptureInput();
        HandleClickedTower();
        if(isDraggingTower)
        {
            Debug.Log("Currently dragging a tower.");
            HandleTowerDragging();
        }
    }

    public void CaptureInput()
    {
        //Debug.Log("CaptureInput called.");
        cameraController.UpdateState();
    }

    public void PrepareToDragTower(TowerAttributes towerAttributes)
    {
        if (towerAttributes == null)
        {
            Debug.LogError("No TowerAttributes provided! Aborting drag operation.");
            return;
        }

        selectedTowerAttributes = towerAttributes;
        isDraggingTower = true;

        // Instantiate a preview of the tower being dragged
        towerPreview = Instantiate(towerAttributes.Prefab);
        towerPreview.SetActive(true); // Make sure it's visible

        Debug.Log($"Started dragging tower: {selectedTowerAttributes.towerName}");
    }

    // public void HandleTowerDragging()
    // {
    //     Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
    //     RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
    //     if (hit.collider != null)
    //     {
    //         towerPreview.transform.position = hit.point;
    //         Debug.Log($"Tower preview moved to position: {hit.point}");
    //
    //         if (Input.GetMouseButtonUp(0))
    //         {
    //             Debug.Log("Left mouse button released, attempting to place tower...");
    //             constructManager.SelectTower(selectedTowerAttributes);
    //             constructManager.PlaceTower(hit.point);
    //             Debug.Log($"Tower placed at position: {hit.point}");
    //             CancelTowerDragging();
    //         }
    //     }
    //
    //     // If right mouse button is clicked, cancel the drag
    //     if (Input.GetMouseButtonUp(1))
    //     {
    //         Debug.Log("Right mouse button released, cancelling tower dragging.");
    //         CancelTowerDragging();
    //     }
    // }

    public void HandleTowerDragging()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, tilemapLayerMask);

        if (hit.collider != null)
        {
            Tilemap tilemap = hit.collider.GetComponent<Tilemap>();
            if (tilemap != null)
            {
                Vector3Int cellPosition = tilemap.WorldToCell(hit.point);
                Vector3 cellCenterPos = tilemap.GetCellCenterWorld(cellPosition);
                towerPreview.transform.position = cellCenterPos;
                Debug.Log($"Tower preview moved to position: {cellCenterPos}");

                if (Input.GetMouseButtonUp(0))
                {
                    Debug.Log("Left mouse button released, attempting to place tower...");
                    constructManager.SelectTower(selectedTowerAttributes);

                        constructManager.PlaceTower(cellCenterPos);
                        Debug.Log($"Tower placed at position: {cellCenterPos}");

                    CancelTowerDragging();
                }
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            Debug.Log("Right mouse button released, cancelling tower dragging.");
            CancelTowerDragging();
        }
    }

// 检查指定位置是否有效
    // private bool IsValidPosition(Vector3 position)
    // {
    //     Collider2D collider = Physics2D.OverlapPoint(position);
    //     return collider == null; // 如果该位置没有碰撞器，位置是有效的
    // }
    private void CancelTowerDragging()
    {
        isDraggingTower = false;
        if (towerPreview != null)
        {
            Destroy(towerPreview);
            Debug.Log("Tower preview destroyed.");
        }

        Debug.Log("Drag operation finished or cancelled.");
    }

    public void HandleClickedTower()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Left mouse button clicked.");
            Vector3? position = GetPositionFromInput();
            if (position.HasValue)
            {
                Debug.Log($"Mouse clicked at world position: {position.Value}");

                // 射线检测以确定点击的对象
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
                if (hit.collider != null)
                {
                    Tower clickedTower = towerManager.GetTowerAt(hit.point);
                    if (clickedTower != null)
                    {
                        Debug.Log($"Tower clicked at position: {position.Value}. Showing tower menu.");
                        uiManager.ShowTowerMenu(clickedTower, position.Value);
                    }
                    else
                    {
                        Debug.Log("No tower found at clicked position.");
                    }
                }
                else
                {
                    Debug.Log("No collider hit detected.");
                }
            }
            else
            {
                Debug.Log("Invalid click position.");
            }
        }
    }

    public Vector3? GetPositionFromInput()
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
                Debug.Log($"Mouse clicked on tile at cell position: {cellPosition}, cell center position: {cellCenterPos}");
                return cellCenterPos;
            }
            else
            {
                Debug.Log("Hit collider does not have a Tilemap component.");
            }
        }
        else
        {
            Debug.Log("No collider hit detected.");
        }

        return null; // 点击的区域无效
    }
}

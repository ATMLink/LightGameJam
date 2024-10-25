using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField] private CameraController cameraController;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private ConstructManager constructManager;
    [SerializeField] private TowerManager towerManager;
    [SerializeField] private UiManager uiManager;

    public LayerMask tilemapLayerMask;
    
    public bool isDraggingTower = false;
    private TowerAttributes selectedTowerAttributes;
    private GameObject towerPreview;

    public void UpdateState()
    {
        //Debug.Log("InputManager UpdateState called.");
        CaptureInput();
        HandleClickedTower();
        if(isDraggingTower)
        {
            //Debug.Log("Currently dragging a tower.");
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
            // Debug.LogError("No TowerAttributes provided! Aborting drag operation.");
            return;
        }

        selectedTowerAttributes = towerAttributes;
        isDraggingTower = true;

        towerPreview = new GameObject("TowerPreview");

        // Add a SpriteRenderer to show the tower's sprite
        SpriteRenderer spriteRenderer = towerPreview.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = towerAttributes.towerSprite;
        
        spriteRenderer.sortingOrder = 10;  

        towerPreview.SetActive(true); // Make sure it's visible

        // Debug.Log($"Started dragging tower: {selectedTowerAttributes.towerName}");
    }


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
                //Debug.Log($"Tower preview moved to position: {cellCenterPos}");

                if (Input.GetMouseButtonUp(0))
                {
                    if (EventSystem.current.IsPointerOverGameObject())
                    {
                        return;
                    }
                    // Debug.Log("Left mouse button released, attempting to place tower...");
                    constructManager.SelectTower(selectedTowerAttributes);

                    constructManager.PlaceTower(cellCenterPos);
                    Debug.Log($"Tower placed at position: {cellCenterPos}");

                    CancelTowerDragging();
                }
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            // Debug.Log("Right mouse button released, cancelling tower dragging.");
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
            // Debug.Log("Tower preview destroyed.");
        }

        // Debug.Log("Drag operation finished or cancelled.");
    }

    public void HandleClickedTower()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Left mouse button clicked.");
            
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            // 获取点击的世界坐标位置
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // 使用 OverlapPoint 检测点击点是否有塔
            LayerMask towerLayerMask = LayerMask.GetMask("Tower");
            Collider2D hitCollider = Physics2D.OverlapPoint(mousePosition, towerLayerMask);

            // 检查点击到的是否为塔
            if (hitCollider != null)
            {
                Tower clickedTower = hitCollider.GetComponent<Tower>();
                if (clickedTower != null)
                {
                    Debug.Log($"Tower clicked at position: {mousePosition}, Tower ID: {clickedTower.towerID}. Showing tower menu.");
                    uiManager.ShowTowerMenu(clickedTower);
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
            // 射线检测以确定点击的对象
            // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            //
            // if (hit.collider != null)
            // {
            //     // 检查点击到的物体是否是塔
            //     Tower clickedTower = hit.collider.GetComponent<Tower>();
            //     if (clickedTower != null)
            //     {
            //         Debug.Log($"Tower clicked at position: {hit.point}, Tower ID: {clickedTower.towerID}. Showing tower menu.");
            //         
            //         uiManager.ShowTowerMenu(clickedTower);
            //     }
            //     else
            //     {
            //         Debug.Log("No tower found at clicked position.");
            //     }
            // }
            // else
            // {
            //     Debug.Log("No collider hit detected.");
            // }
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
                // Debug.Log($"Mouse clicked on tile at cell position: {cellPosition}, cell center position: {cellCenterPos}");
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

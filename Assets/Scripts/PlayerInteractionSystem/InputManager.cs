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

    private bool isDraggingTower = false;
    private TowerAttributes selectedTowerAttributes;
    private GameObject towerPreview;

    public void UpdateState()
    {
        CaptureInput();
        HandleClickedTower();
        if(isDraggingTower)
            HandleTowerDragging();
    }

    public void CaptureInput()
    {
        cameraController.UpdateState();
    }

    public void PrepareToDragTower(TowerAttributes towerAttributes)
    {
        if (towerAttributes == null)
        {
            Debug.LogError("No TowerAttributes provided!");
            return;
        }

        selectedTowerAttributes = towerAttributes;
        isDraggingTower = true;

        // Instantiate a preview of the tower being dragged
        towerPreview = Instantiate(towerAttributes.Prefab);
        towerPreview.SetActive(true); // Make sure it's visible

        Debug.Log($"Started dragging tower: {selectedTowerAttributes.towerName}");
    }
    public void HandleTowerDragging()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            towerPreview.transform.position = hit.point;
            
                // When left mouse button is released, build the tower
            if (Input.GetMouseButtonUp(0))
            {
                Debug.Log("Attempting to place tower...");
                constructManager.SelectTower(selectedTowerAttributes);
                constructManager.PlaceTower(hit.point);
                CancelTowerDragging();
            }
        }

        // If right mouse button is clicked, cancel the drag
        if (Input.GetMouseButtonUp(1))
        {
            Debug.Log("Cancelled tower dragging.");
            CancelTowerDragging();
        }
        
    }
    
    private void CancelTowerDragging()
    {
        isDraggingTower = false;
        if (towerPreview != null)
        {
            Destroy(towerPreview);
        }

        Debug.Log("Drag operation finished or cancelled.");
    }

    public void HandleClickedTower()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3? position = GetPositionFromInput();
            if (position.HasValue)
            {
                Tower clickedTower = towerManager.GetTowerAt(position.Value);
                if (clickedTower != null)
                {
                    // 点击到塔，显示 UI 菜单
                    uiManager.ShowTowerMenu(clickedTower, position.Value);
                }
            }
        }
    }

    public void StartDraggingTower(TowerAttributes towerAttributes)
    {
        isDraggingTower = true;
        selectedTowerAttributes = towerAttributes;
        constructManager.SelectTower(towerAttributes);

        // 创建预览塔
        towerPreview = new GameObject("TowerPreview");
        SpriteRenderer renderer = towerPreview.AddComponent<SpriteRenderer>();
        renderer.sprite = towerAttributes.towerSprite; // 设置预览塔的图片
        renderer.color = new Color(1, 1, 1, 0.5f); // 半透明显示
    }

    public void StopDragTower()
    {
        isDraggingTower = false;
        Destroy(towerPreview);
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
                return cellCenterPos;
            }
        }

        return null; // 点击的区域无效
    }
    
}

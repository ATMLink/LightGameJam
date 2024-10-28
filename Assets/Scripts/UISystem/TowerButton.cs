using UnityEngine;
using UnityEngine.EventSystems;

public class TowerButton : MonoBehaviour, IPointerDownHandler
    {
    [SerializeField] private TowerAttributes towerAttributes;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private GameObject informationImage;
    [SerializeField] private MusicManager musicManager;
    [SerializeField] private GameObject ShowTowerMenuPanel;
    // 当鼠标按下时调用
    public void OnPointerDown(PointerEventData eventData)
        {
        ShowTowerMenuPanel.SetActive(false);
        if (musicManager != null)
            musicManager.PlaySound("TowerClick");
        informationImage.SetActive(false);
        inputManager.PrepareToDragTower(towerAttributes); // 立即开始拖拽

        }
    }
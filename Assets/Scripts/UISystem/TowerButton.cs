using UnityEngine;
using UnityEngine.EventSystems;

public class TowerButton : MonoBehaviour, IPointerDownHandler
    {
    [SerializeField] private TowerAttributes towerAttributes;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private GameObject informationImage;
    [SerializeField] private MusicManager musicManager;

    // 当鼠标按下时调用
    public void OnPointerDown(PointerEventData eventData)
        {
        if (musicManager != null)
            musicManager.PlaySound("TowerClick");
        informationImage.SetActive(false);
        inputManager.PrepareToDragTower(towerAttributes); // 立即开始拖拽

        }
    }
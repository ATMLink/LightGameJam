using UnityEngine;
using UnityEngine.EventSystems;

public class TowerButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private TowerAttributes towerAttributes;
    [SerializeField] private InputManager inputManager;

    // 当鼠标按下时调用
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Button pressed, preparing to drag tower.");
        inputManager.PrepareToDragTower(towerAttributes); // 立即开始拖拽
    }
}
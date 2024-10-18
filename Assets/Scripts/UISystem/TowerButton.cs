using UnityEngine;
using UnityEngine.EventSystems;

public class TowerButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private TowerAttributes towerAttributes;
    [SerializeField] private InputManager inputManager;

    // ����갴��ʱ����
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Button pressed, preparing to drag tower.");
        inputManager.PrepareToDragTower(towerAttributes); // ������ʼ��ק
    }
}
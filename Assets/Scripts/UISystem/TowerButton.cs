using UnityEngine;
using UnityEngine.EventSystems;

public class TowerButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private TowerAttributes towerAttributes;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private GameObject informationImage;

    // ����갴��ʱ����
    public void OnPointerDown(PointerEventData eventData)
    {
        informationImage.SetActive(false);
        Debug.Log("Button pressed, preparing to drag tower.");
        inputManager.PrepareToDragTower(towerAttributes); // ������ʼ��ק

    }
}
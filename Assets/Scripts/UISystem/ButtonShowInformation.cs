using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonShowInformation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{


    public Image hoverSprite; // ÐüÍ£Ê±ÏÔÊ¾µÄÍ¼Æ¬
    [SerializeField] private InputManager inputManager;
 
    void Start()
    {
        hoverSprite.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
      
        hoverSprite.gameObject.SetActive(!inputManager.isDraggingTower);
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverSprite.gameObject.SetActive(false);
    }
}
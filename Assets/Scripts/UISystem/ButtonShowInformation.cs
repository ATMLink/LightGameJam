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
        if (hoverSprite != null)
        hoverSprite.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverSprite != null)
            hoverSprite.gameObject.SetActive(!inputManager.isDraggingTower);
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (hoverSprite != null)
            hoverSprite.gameObject.SetActive(false);
    }
}
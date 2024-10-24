using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonShowInformation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{


    public Image hoverSprite; // 悬停时显示的图片

    void Start()
    {
        hoverSprite.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
        hoverSprite.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 鼠标离开时恢复原始图片
        hoverSprite.gameObject.SetActive(false);
    }
}
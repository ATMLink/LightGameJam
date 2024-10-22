using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerShowInformation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{


    public Image displayImage; // 用于显示的Image对象
    public Sprite hoverSprite; // 悬停时显示的图片
    private Sprite originalSprite; // 原始图片

    void Start()
    {
        // 保存原始图片
        originalSprite = displayImage.sprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // 鼠标悬停时更改图片
        displayImage.sprite = hoverSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 鼠标离开时恢复原始图片
        displayImage.sprite = originalSprite;
    }
}

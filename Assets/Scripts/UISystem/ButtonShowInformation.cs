using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonShowInformation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{


    public Image hoverSprite; // ��ͣʱ��ʾ��ͼƬ

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
        // ����뿪ʱ�ָ�ԭʼͼƬ
        hoverSprite.gameObject.SetActive(false);
    }
}
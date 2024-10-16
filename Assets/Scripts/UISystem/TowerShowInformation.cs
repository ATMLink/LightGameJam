using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor.UI;
using UnityEngine.UI;

public class TowerShowInformation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{


    public Image displayImage; // ������ʾ��Image����
    public Sprite hoverSprite; // ��ͣʱ��ʾ��ͼƬ
    private Sprite originalSprite; // ԭʼͼƬ

    void Start()
    {
        // ����ԭʼͼƬ
        originalSprite = displayImage.sprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // �����ͣʱ����ͼƬ
        displayImage.sprite = hoverSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // ����뿪ʱ�ָ�ԭʼͼƬ
        displayImage.sprite = originalSprite;
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum element {k,si,cu,na,li,ca }
public class MainResourceManagement : MonoBehaviour
{
    [SerializeField] private FloatVariable siNumber;
    [SerializeField] private FloatVariable kNumber;
    [SerializeField] private FloatVariable naNumber;
    [SerializeField] private FloatVariable cuNumber;
    [SerializeField] private FloatVariable caNumber;
    [SerializeField] private FloatVariable liNumber;
    /// <summary>
    /// ������Ԫ������������Ϊ0
    /// </summary>
    public void Initialize() 
    {
        siNumber.SetValue(0);
        kNumber.SetValue(0);            
        naNumber.SetValue(0);
        cuNumber.SetValue(0);
        liNumber.SetValue(0);
        caNumber.SetValue(0);
    }
    public void Initialize(element element)
    {
        FloatVariable _element = null;
        switch (element)
        {
            case element.si: _element = siNumber; break;
            case element.k: _element = kNumber; break;
            case element.cu: _element = cuNumber; break;
            case element.na: _element = naNumber; break;
            case element.li: _element = liNumber; break;
            case element.ca: _element = caNumber; break;
        }
        _element.SetValue(0);
    }
    /// <summary>
    /// ������Ҫ�жϵ�Ԫ�ؼ�������������true��false
    /// </summary>
    public bool JudgeAfford( element element,float number) 
    {
        float elementNumber = 0;
        switch (element) { 
            case element.si:elementNumber = siNumber.Value; break;            
            case element.k:elementNumber = kNumber.Value; break;        
            case element.cu:elementNumber = cuNumber.Value; break;     
            case element.na:elementNumber = naNumber.Value; break;
            case element.li:elementNumber = liNumber.Value; break;
            case element.ca:elementNumber = caNumber.Value; break;
        }
        bool isAfford = ( elementNumber > number )?  true : false;
        return isAfford;
    }
    /// <summary>
    /// ������Ҫ�жϵ�Ԫ�ؼ�����������������㹻����������ӦԪ�ص�����������ִ��
    /// </summary>
    public void SpendResoure(element element, float number) {
        if (number <= 0) { Debug.LogError("spend number must bigger than 0"); return; };
        if (JudgeAfford(element,number )) {
            FloatVariable _element = null;
            switch (element)
            {
                case element.si: _element = siNumber; break;
                case element.k: _element = kNumber; break;
                case element.cu: _element = cuNumber; break;
                case element.na: _element = naNumber; break;
                case element.li:_element = liNumber; break;
                case element.ca:_element = caNumber; break;
            }
            _element.SetValue(_element.Value-number);
        }        
    }
    /// <summary>
    /// ����Ԫ�����࣬�������ӦԪ�ص�����
    /// </summary>
    public float GetResourceNumber(element element) {
        float elementNumber = 0;
        switch (element)
        {
            case element.si: elementNumber = siNumber.Value; break;
            case element.k: elementNumber = kNumber.Value; break;
            case element.cu: elementNumber = cuNumber.Value; break;
            case element.na: elementNumber = naNumber.Value; break;
            case element.li: elementNumber = liNumber.Value;break;
            case element.ca: elementNumber = caNumber.Value; break; 
        }
        return elementNumber;
    }
    public void CollectResource(element element, float number) {
        if (number <= 0) { Debug.LogError("collect number must bigger than 0");return; };
        FloatVariable _element = null;
        switch (element)
        {
            case element.si: _element = siNumber; break;
            case element.k: _element = kNumber; break;
            case element.cu: _element = cuNumber; break;
            case element.na: _element = naNumber; break;
            case element.li:_element = liNumber; break;
            case element.ca:_element = caNumber; break;
        }
        _element.SetValue(_element.Value + number);


    }
}
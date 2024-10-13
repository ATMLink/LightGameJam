using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TowerGame/Variables/Float",
    fileName = "FloatVariable",
    order = 0)]
public class FloatVariable : ScriptableObject
{
    public float Value;

    public GameEventFloat valueChangedEvent;

    public void SetValue(float value)
    {
        Value = value;
        if(valueChangedEvent != null)
            valueChangedEvent.Raise(value);
    }
}

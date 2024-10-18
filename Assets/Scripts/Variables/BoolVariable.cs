using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TowerGame/Variables/Bool",
    fileName = "BoolVariable",
    order = 2)]
public class BoolVariable : ScriptableObject
{
    public bool Value;

    public GameEventBool valueChangedEvent;

    public void SetValue(bool value)
    {
        Value = value;
        if(valueChangedEvent != null)
            valueChangedEvent.Raise(value);
    }
}

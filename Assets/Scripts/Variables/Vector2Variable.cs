using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TowerGame/Variables/Vector2",
    fileName = "Vector2Variable",
    order = 1)]
public class Vector2Variable : ScriptableObject
{
    public Vector2 Value;

    public GameEventVector2 valueChangedEvent;

    public void SetValue(Vector2 value)
    {
        Value = value;
        if(valueChangedEvent != null)
            valueChangedEvent.Raise(value);
    }
}

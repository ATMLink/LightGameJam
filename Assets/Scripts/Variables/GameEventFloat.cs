using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TowerGame/Events/Game event(float)",
    fileName = "GameEventFloat",
    order = 0)]

public class GameEventFloat : ScriptableObject
{
    private readonly List<GameEventFloatListener> _listeners = new List<GameEventFloatListener>();

    public void Raise(float value)
    {
        for (var i = _listeners.Count - 1; i >= 0; i--)
        {
            _listeners[i].OnEventRaised(value);
        }
    }
    
    public void RegisterListener(GameEventFloatListener listener)
    {
        if (!_listeners.Contains(listener))
        {
            _listeners.Add(listener);
        }
    }

    public void UnRegisterListener(GameEventFloatListener listener)
    {
        if (_listeners.Contains(listener))
        {
            _listeners.Remove(listener);
        }
    }
}

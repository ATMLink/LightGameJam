using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "TowerGame/Events/Game event(Bool)",
    fileName = "GameEventBool",
    order = 2)]
public class GameEventBool : ScriptableObject
{
    private readonly List<GameEventBoolListener> _listeners = new List<GameEventBoolListener>();

    public void Raise(bool value)
    {
        for (var i = _listeners.Count - 1; i >= 0; i--)
        {
            _listeners[i].OnEventRaised(value);
        }
    }
    
    public void RegisterListener(GameEventBoolListener listener)
    {
        if (!_listeners.Contains(listener))
        {
            _listeners.Add(listener);
        }
    }

    public void UnRegisterListener(GameEventBoolListener listener)
    {
        if (_listeners.Contains(listener))
        {
            _listeners.Remove(listener);
        }
    }
}

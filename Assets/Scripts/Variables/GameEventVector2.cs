using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "TowerGame/Events/Game event(Vector2)",
    fileName = "GameEventVector2",
    order = 1)]
public class GameEventVector2 : ScriptableObject
{
    private readonly List<GameEventVector2Listener> _listeners = new List<GameEventVector2Listener>();

    public void Raise(Vector2 value)
    {
        for (var i = _listeners.Count - 1; i >= 0; i--)
        {
            _listeners[i].OnEventRaised(value);
        }
    }
    
    public void RegisterListener(GameEventVector2Listener listener)
    {
        if (!_listeners.Contains(listener))
        {
            _listeners.Add(listener);
        }
    }

    public void UnRegisterListener(GameEventVector2Listener listener)
    {
        if (_listeners.Contains(listener))
        {
            _listeners.Remove(listener);
        }
    }
}

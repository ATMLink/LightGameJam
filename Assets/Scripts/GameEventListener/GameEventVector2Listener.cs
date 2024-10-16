using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventVector2Listener : MonoBehaviour
{
    // listening event
    public GameEventVector2 eventVector2;
    
    //react when event raised
    public UnityEvent<Vector2> response;

    private void OnEnable()
    {
        eventVector2.RegisterListener(this);
    }

    private void OnDisable()
    {
        eventVector2.UnRegisterListener(this);
    }

    public void OnEventRaised(Vector2 value)
    {
        response.Invoke(value);
    }
}

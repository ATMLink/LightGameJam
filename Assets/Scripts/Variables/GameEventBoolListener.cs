using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class GameEventBoolListener : MonoBehaviour
{
    // listening event
    public GameEventBool eventFloat;
    
    //react when event raised
    public UnityEvent<bool> response;

    private void OnEnable()
    {
        eventFloat.RegisterListener(this);
    }

    private void OnDisable()
    {
        eventFloat.UnRegisterListener(this);
    }

    public void OnEventRaised(bool value)
    {
        response.Invoke(value);
    }
}

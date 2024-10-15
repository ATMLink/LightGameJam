using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventFloatListener : MonoBehaviour
{
    // listening event
    public GameEventFloat eventFloat;
    
    //react when event raised
    public UnityEvent<float> response;

    private void OnEnable()
    {
        eventFloat.RegisterListener(this);
    }

    private void OnDisable()
    {
        eventFloat.UnRegisterListener(this);
    }

    public void OnEventRaised(float value)
    {
        response.Invoke(value);
    }
}

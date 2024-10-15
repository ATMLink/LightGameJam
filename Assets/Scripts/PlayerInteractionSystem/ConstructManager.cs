using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructManager : MonoBehaviour
{
    public void Initialize()
    {
        // resourceManager = FindObjectOfType<ResourceManager>();
    }

    public void PlaceTower(Vector3 hitPoint)
    {
        // build a tower if afford
        Debug.Log($"built a tower at {hitPoint}");
    }
}

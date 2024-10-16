using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TileType : ScriptableObject
{
    public Sprite Sprite;
    public bool canThrough;
    public bool canConstruct;
    public Vector3Int tilePosition;
    public string tileName;
    
    
}

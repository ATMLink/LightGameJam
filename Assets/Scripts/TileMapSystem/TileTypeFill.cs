using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTypeFill : ScriptableObject
{
    public Sprite Sprite;
    public bool canThrough;
    public bool canConstruct;
    public Vector3Int startTilePosition;
    public Vector3Int endTilePosition;   
    public string tileName;
}

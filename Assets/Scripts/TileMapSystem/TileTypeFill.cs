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
    public Vector3 tileSize;
    public Vector3 tileScale;
    public Vector3 tileColor;    
    public string tileName;
}

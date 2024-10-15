using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapTrigger : MonoBehaviour
{
    public Tilemap tilemap;
    private void OnTriggerStay2D(Collider2D other)
    {
        Vector3Int tilePosition = tilemap.WorldToCell(other.transform.position);
        TileBase tile = tilemap.GetTile(tilePosition);
        if (tile != null) {
            switch (tile.name) { 
            
            
            }
        
        }
    }

}

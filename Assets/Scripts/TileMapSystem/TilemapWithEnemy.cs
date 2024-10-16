using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapWithEnemy : MonoBehaviour
{
    public string tileName;
    public Sprite Sprite;
    public bool canThrough;
    public bool canConstruct;
    public Vector3Int tilePosition;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag =="Enemy") { }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy") { }
    }

}

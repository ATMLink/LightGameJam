using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapWithEnemy : MonoBehaviour
{
    public string tileName;
    public Sprite Sprite;
    public bool canLightThrough;
    public bool canEnemyThrough;
    public bool canConstruct;
    public Vector3 tilePosition;
    private void Start()
    {
        tilePosition = transform.position;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log(111);
        if (collision.tag =="Enemy") { }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy") { }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapWithEnemy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag =="Enemy") { }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy") { }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TilemapFeature : MonoBehaviour
{
    public string tileName;
    public Vector3 tilePosition;
    public List<Sprite> sprites;
    public bool canLightThrough;
    public bool canEnemyThrough;
    public bool canConstruct;
    public bool canSlowEnemy;
    public bool canAttackTowerConstruct;
    public bool canMinerConstruct;
    [SerializeField]private ShadowCaster2D shadowCaster;
    [SerializeField] private SpriteRenderer sprite;
    private void Start()
    {
        shadowCaster = GetComponent<ShadowCaster2D>();
        tilePosition = transform.position;
        if (!canLightThrough)shadowCaster.enabled = false;
    }
    public void ChangeSprite(int number) { 
        sprite.sprite = sprites[number];
    
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

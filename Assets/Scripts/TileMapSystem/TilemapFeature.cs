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
    [SerializeField]protected ShadowCaster2D shadowCaster;
    [SerializeField]protected SpriteRenderer sprite;
    private void Start()
    {
        shadowCaster = GetComponent<ShadowCaster2D>();
        tilePosition = transform.position;
        sprite = GetComponent<SpriteRenderer>();
        if (sprites[0]!=null)
        sprite.sprite = sprites[0];
        if (canLightThrough)shadowCaster.enabled = false;
    }
    public void ChangeSprite(int number) { 
        sprite.sprite = sprites[number];  
    }
}

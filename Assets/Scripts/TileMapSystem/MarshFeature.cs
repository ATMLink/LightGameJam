using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MarshFeature : MonoBehaviour
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
        sprite = GetComponent<SpriteRenderer>();
        if (sprites[0]!=null)
        sprite.sprite = sprites[0];
        if (canLightThrough)shadowCaster.enabled = false;
    }
    public void ChangeSprite(int number) { 
        sprite.sprite = sprites[number];
    
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && canSlowEnemy)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.SetSpeed(0.5f);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && canSlowEnemy) {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.SetSpeed(1f);
            }
        }
    }

}

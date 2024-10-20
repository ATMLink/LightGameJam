using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(fileName = "New Tile", menuName = "TileType/TileTypeRandom", order = 2)]
public class TileTypeRandom : ScriptableObject
{
    [Header("Name")]
    public string tileName;
    [Header("Sprite")]
    public List<Sprite> sprites;
    //[Header("CanThrough")]
    //public bool canLightThrough;
    //public bool canEnemyThrough;
    //[Header("Slow")]
    //public bool canSlowEnemy;
    //[Header("CanConstruct")]
    //public bool canConstruct;
    //public bool canAttackTowerConstruct;
    //public bool canMinerConstruct;

    //[Header("Modification")]
    //public Vector3Int tilePosition;




}

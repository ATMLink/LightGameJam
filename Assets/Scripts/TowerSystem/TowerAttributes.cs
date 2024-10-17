using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    menuName = "TowerGame/Towers/TowerAttributes",
    fileName = "TowerAttributes",
    order = 0)]
public class TowerAttributes : ScriptableObject
{
    public FloatVariable health;
    public FloatVariable damage;
    public FloatVariable attackSpeed;
    public FloatVariable attackRange;

    public TowerAttributes nextLevelAttributes;
}

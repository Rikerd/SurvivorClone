using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Character Stat", menuName = "Stats/Player Character")]
public class PlayerCharacterStats : Stats
{
    [Range(0f, 1f)]
    public float critChance;

    public float baseAttack;

    [Header("Level Up Increase Rates")]
    public float attackLevelRate;
    public float attackSpeedRate;
    public float movementSpeedRate;
    public float critChanceRate;
    public int healthRate;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Character Stat", menuName = "Stats/Player Character")]
public class PlayerCharacterStats : Stats
{
    [Range(0f, 1f)]
    public float critChance;
}

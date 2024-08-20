using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Stat", menuName = "Stats/Weapon/Basic Weapon")]
public class WeaponStats : ScriptableObject
{
    public bool canCrit;
}

[Serializable]
public class WeaponLevelStats
{
    public int minDamage;
    public int maxDamage;
    public float maxCooldown;
}
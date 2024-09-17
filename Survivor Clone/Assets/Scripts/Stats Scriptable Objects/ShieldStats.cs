using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shield Stat", menuName = "Stats/Weapon/Shield")]
public class ShieldStats : WeaponStats
{
    public List<ShieldLevelStats> levelStats;
}

[Serializable]
public class ShieldLevelStats
{
    public float maxCooldown;
    public float maxCharges;
    public float invincibilityTime;
}

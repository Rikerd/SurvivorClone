using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Aura Stat", menuName = "Stats/Weapon/Aura")]
public class AuraStats : WeaponStats
{
    public List<AuraLevelStats> levelStats;
}

[Serializable]
public class AuraLevelStats: WeaponLevelStats
{
    public float radius;
}

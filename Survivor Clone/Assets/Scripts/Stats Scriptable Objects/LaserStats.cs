using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Laser Stat", menuName = "Stats/Weapon/Laser")]
public class LaserStats : WeaponStats
{
    public List<WeaponLevelStats> levelStats;
}
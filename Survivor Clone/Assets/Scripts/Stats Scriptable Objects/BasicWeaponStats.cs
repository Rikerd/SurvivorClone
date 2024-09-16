using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Stat", menuName = "Stats/Weapon/Basic Weapon")]
public class BasicWeaponStats : WeaponStats
{
    public List<WeaponLevelStats> levelStats;
}
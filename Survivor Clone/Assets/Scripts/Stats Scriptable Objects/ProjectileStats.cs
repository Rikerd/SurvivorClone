using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile Stat", menuName = "Stats/Weapon/Projectile")]
public class ProjectileStats : WeaponStats
{
    public float moveSpeedRatio;
    public GameObject projectile;

    public List<ProjectileLevelStats> levelStats;
}

[Serializable]
public class ProjectileLevelStats : WeaponLevelStats
{
    public int projectileCount;

    [Tooltip("Set to -1 if always piercing")]
    public int pierceAmount;
}
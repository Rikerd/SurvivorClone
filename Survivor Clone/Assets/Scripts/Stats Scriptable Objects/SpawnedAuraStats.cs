using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spawned Aura Stat", menuName = "Stats/Weapon/Spawned Aura")]
public class SpawnedAuraStats : WeaponStats
{
    public List<SpawnedAuraLevelStats> levelStats;
}

[Serializable]
public class SpawnedAuraLevelStats : AuraLevelStats
{
    public float lifeTime;
}
